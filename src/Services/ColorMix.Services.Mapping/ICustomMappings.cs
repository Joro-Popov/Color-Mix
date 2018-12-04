using AutoMapper;

namespace ColorMix.Services.Mapping
{
    public interface ICustomMappings
    {
        void CreateMappings(IMapperConfigurationExpression configuration);
    }
}
