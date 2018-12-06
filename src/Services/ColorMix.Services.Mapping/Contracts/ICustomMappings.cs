using AutoMapper;

namespace ColorMix.Services.Mapping.Contracts
{
    public interface ICustomMappings
    {
        void CreateMappings(IMapperConfigurationExpression configuration);
    }
}
