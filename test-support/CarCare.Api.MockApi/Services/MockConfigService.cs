namespace CarCare.Api.MockApi.Services;

public class MockConfigService<T>
{
    private readonly Queue<MockConfig<T>> _responses;

    public MockConfigService()
    {
        _responses = new();
    }

    public void EnqueueResponse(T response, int statusCode)
    {
        _responses.Enqueue(new MockConfig<T>(response, statusCode));
    }

    public MockConfig<T> DequeueResponse()
    {
        if (_responses.Count == 0)
        {
            throw new InvalidOperationException("No mock responses available.");
        }

        return _responses.Dequeue();
    }
}

public sealed record MockConfig<T>(T Response, int StatusCode);