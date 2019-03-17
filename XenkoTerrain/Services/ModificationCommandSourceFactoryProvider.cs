namespace XenkoTerrain.Services
{
  public static class ModificationCommandSourceFactoryProvider
  {
    public static IModificationCommandFactory Factory { get; private set; }

    public static void SetFactory(IModificationCommandFactory factory)
    {
      Factory = factory;
    }
  }
}