
export interface textConverterResponse {
    readonly result: string;
    readonly jobId: string;
}

export interface TextConverterRequest {
    readonly text: string;
}
