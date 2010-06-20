namespace ProxyGen.ServiceGenerator
{
    public interface IServiceCodeGenerator
    {
        void Initialize();

        void Prepare();

        void Write();
    }
}