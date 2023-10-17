
export interface TextConverterResponse {
    readonly jobId: string;
}

export interface StartTextConversionRequest {
    readonly text: string;
}

export interface CancelTextConversionRequest {
    readonly jobId: string;
}