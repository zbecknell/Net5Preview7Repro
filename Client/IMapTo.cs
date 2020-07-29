using AutoMapper;

namespace Net5Preview7Repro
{
    public interface IMapTo<T>
    {
        void Mapping(Profile profile)
        {
            var mappingExpression = profile.CreateMap(typeof(T), GetType())
                .ReverseMap();

            mappingExpression
                .ForAllOtherMembers(opt => opt.Condition(
                    (source, destination, sourceMember, destMember) => (sourceMember != null)));
        }
    }
}
