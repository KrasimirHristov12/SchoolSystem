namespace SchoolSystem.Services.Mapping
{
    public static class MapObjectExtensions
    {
        public static T Map<T>(this IMapObject obj)
        {
            return AutoMapperConfig.MapperInstance.Map<T>(obj);
        }
    }
}
