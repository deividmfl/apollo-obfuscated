namespace PhantomInterop.Interfaces
{
    public interface FlexibleCoordinator3B79
    {
        bool AddEgress(IC2Profile profile);
        bool AddIngress(IC2Profile profile);

        IC2Profile[] GetEgressCollection();
        IC2Profile[] GetIngressCollection();

        IC2Profile[] GetConnectedEgressCollection();
    }
}
