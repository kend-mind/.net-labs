using AutoMapper;
using VPBANK.RMD.Utils.Common.Datas;

namespace VPBANK.RMD.API.Common.Helpers.Mapping
{
    public class ModelMappingProfile : Profile
    {
        /// <summary>
        /// Create automap mapping profiles
        /// </summary>
        public ModelMappingProfile()
        {
            // Add as many of these lines as you need to map your objects
            CreateMap<FileUploadInfo, FileUploadReq>()
                .ForMember(dest => dest.Filename, opts => opts.MapFrom(src => src.filename))
                .ForMember(dest => dest.Path, opts => opts.MapFrom(src => src.path))
                .ForMember(dest => dest.HostInfo, opts => opts.MapFrom(src => src.hostInfo))
                .ForMember(dest => dest.FileType, opts => opts.MapFrom(src => src.fileType))
                .ForMember(dest => dest.TargetServer, opts => opts.MapFrom(src => src.targetServer))
                .ForMember(dest => dest.DocumentType, opts => opts.Ignore());
            CreateMap<FileUploadReq, FileUploadInfo>()
                .ForMember(dest => dest.filename, opts => opts.MapFrom(src => src.Filename))
                .ForMember(dest => dest.path, opts => opts.MapFrom(src => src.Path))
                .ForMember(dest => dest.hostInfo, opts => opts.MapFrom(src => src.HostInfo))
                .ForMember(dest => dest.fileType, opts => opts.MapFrom(src => src.FileType))
                .ForMember(dest => dest.targetServer, opts => opts.MapFrom(src => src.TargetServer))
                .ForMember(dest => dest.size, opts => opts.Ignore())
                .ForMember(dest => dest.updateDt, opts => opts.Ignore());


            //CreateMap<AnswerSetViewModel, AnswerSet>()
            //    .ForMember(dest => dest.Date, opts => opts.MapFrom(src => src.DateAnswersSubmitted))
            //    .ForMember(dest => dest.Id, opts => opts.MapFrom(src => src.AnswerSetId))
            //    .ForMember(dest => dest.Answers, opts => opts.MapFrom(src => src.AnswerSetAnswers))
            //    .ForMember(dest => dest.IsActive, opts => opts.Ignore());
            //CreateMap<AnswerSet, AnswerSetViewModel>()
            //    .ForMember(dest => dest.DateAnswersSubmitted, opts => opts.MapFrom(src => src.Date))
            //    .ForMember(dest => dest.AnswerSetId, opts => opts.MapFrom(src => src.Id))
            //    .ForMember(dest => dest.AnswerSetAnswers, opts => opts.MapFrom(src => src.Answers));

            //CreateMissingTypeMaps = true;
        }
    }
}
