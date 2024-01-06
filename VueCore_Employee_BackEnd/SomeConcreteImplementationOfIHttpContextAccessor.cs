public class SomeConcreteImplementationOfIHttpContextAccessor : IHttpContextAccessor
{
    public HttpContext? HttpContext { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
}