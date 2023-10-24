using AutoFixture.Xunit2;
using FluentAssertions;
using LongRunningJobImitator.Accessors.Interfaces;
using LongRunningJobImitator.Accessors.Models;
using LongRunningJobImitator.Services.Interfaces;
using LongRunningJobImitator.Services.Services;
using LongRunningJobImitator.Services.Tests.AutoFixtureConfiguration;
using MongoDB.Driver;
using Moq;
using System.Text;

namespace LongRunningJobImitator.Services.Tests.Services;
public class LongRunningConversionWorkerTests
{
    [Theory]
    [AutoMoqData]
    public async void StartJobAsync_SimpleText_AllResultsReported(
        [Frozen] Mock<ITextConversionResultSender> resultSenderMock,
        [Frozen] Mock<IJobAccessor> accessorMock,
        [Frozen] Mock<ITextEncoder> encoderMock,
        [Frozen] Mock<UpdateResult> updateResultMock,
        Guid jobId,
        LongRunningConversionWorker sut)
    {
        // Arrange
        var doc = GetFakeDoc(jobId);
        var expected = doc.Result;
        var actual = new StringBuilder();

        ConfigureAccessor(accessorMock, updateResultMock, doc);
        encoderMock.Setup(x => x.Encode(new(doc.Text))).Returns(expected);
        resultSenderMock.Setup(x => x.SendResultAsync(It.IsAny<Guid>(), It.IsAny<string>(), default))
            .Callback<Guid, string, CancellationToken>((_, value, _) => actual.Append(value))
            .Returns(Task.CompletedTask);

        // Act
        await sut.StartJobAsync(new(jobId), default);

        // Assert
        actual.ToString().Should().Be(expected);
    }

    [Theory]
    [AutoMoqData]
    public async void StartJobAsync_SimpleText_DoneResultSent(
        [Frozen] Mock<ITextConversionResultSender> resultSenderMock,
        [Frozen] Mock<IJobAccessor> accessorMock,
        [Frozen] Mock<UpdateResult> updateResultMock,
        Guid jobId,
        LongRunningConversionWorker sut)
    {
        // Arrange
        var doc = GetFakeDoc(jobId);
        ConfigureAccessor(accessorMock, updateResultMock, doc);

        // Act
        await sut.StartJobAsync(new(jobId), default);

        // Assert
        resultSenderMock.Verify(x => x.SendDoneAsync(jobId, default), Times.Once());
    }

    private static JobDoc GetFakeDoc(Guid jobId)
    {
        return new JobDoc(jobId, JobStatus.NotStarted, "Test text", "Test result", 0);
    }

    private static void ConfigureAccessor(Mock<IJobAccessor> accessorMock, Mock<UpdateResult> updateResultMock, JobDoc doc)
    {
        updateResultMock.SetupGet(x => x.MatchedCount).Returns(1);
        updateResultMock.SetupGet(x => x.ModifiedCount).Returns(1);
        accessorMock.Setup(x => x.GetAsync(It.IsAny<Guid>(), default))
            .ReturnsAsync(doc);
    }
}
