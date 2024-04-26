﻿namespace ModelLayer.Response;

public class ResponseModel<T>
{
    public bool Success { get; set; } = true;
    public string Message { get; set; } = string.Empty;
    public T? Data { get; set; }

}