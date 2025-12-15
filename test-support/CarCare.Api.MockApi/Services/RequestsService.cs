using CarCare.Api.MockApi.Models;

namespace CarCare.Api.MockApi.Services;

public class RequestsService
{
    private readonly Queue<Request> _requests;

    public RequestsService()
    {
        _requests = new();
    }

    public void EnqueueRequest(Request request)
    {
        _requests.Enqueue(request);
    }

    public Request DequeueRequest()
    {
        if (_requests.Count == 0)
        {
            throw new InvalidOperationException("No mock requests available.");
        }
        return _requests.Dequeue();
    }
}
