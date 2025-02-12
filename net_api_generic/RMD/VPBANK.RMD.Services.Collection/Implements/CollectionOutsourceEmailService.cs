using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VPBANK.RMD.Data.Collection;
using VPBANK.RMD.Data.Collection.Entities.POCOs;
using VPBANK.RMD.Data.Collection.Views;
using VPBANK.RMD.EFCore;
using VPBANK.RMD.EFCore.Generics;
using VPBANK.RMD.Repositories.Collection.Interfaces;
using VPBANK.RMD.Services.Collection.Interfaces;
using VPBANK.RMD.Utils.Common;
using VPBANK.RMD.Utils.Common.Datas;
using VPBANK.RMD.Utils.Common.Shared;
using VPBANK.RMD.Utils.Common.Validates;

namespace VPBANK.RMD.Services.Collection.Implements
{
    public class CollectionOutsourceEmailService : ICollectionOutsourceEmailService
    {
        private readonly IUnitOfWork<CollectionContext> _unitOfWork;
        private readonly IGenericRepository<CollectionContext, CollectionEmail, int> _genCollecEmailRepository;
        private readonly IGenericRepository<CollectionContext, CollectionEmailFileAttach, int> _genCollecEmailAttachRepository;
        private readonly ICollectionEmailFileAttachRepository _collecEmailAttachRepository;

        public CollectionOutsourceEmailService(IUnitOfWork<CollectionContext> unitOfWork,
            IGenericRepository<CollectionContext, CollectionEmail, int> genCollecEmailRepository,
            IGenericRepository<CollectionContext, CollectionEmailFileAttach, int> genCollecEmailAttachRepository,
            ICollectionEmailFileAttachRepository collecEmailAttachrepository)
        {
            _unitOfWork = unitOfWork;
            _genCollecEmailRepository = genCollecEmailRepository;
            _genCollecEmailAttachRepository = genCollecEmailAttachRepository;
            _collecEmailAttachRepository = collecEmailAttachrepository;
        }

        public async Task InsertAsync(ViewCollectionEmailDto entity)
        {
            try
            {
                _unitOfWork.BeginTransaction();

                var collecEmail = new CollectionEmail
                {
                    Pk_Id = 0,
                    Os_Company = entity.Os_Company,
                    Segment = entity.Segment,
                    From = entity.From,
                    To = entity.To,
                    Cc = entity.Cc,
                    Bcc = entity.Bcc,
                    Subject = entity.Subject,
                    Fk_Email_Tempt_Id = entity.Fk_Email_Tempt_Id,
                    Recipient_Email = entity.Recipient_Email
                };
                _genCollecEmailRepository.Insert(collecEmail);
                await _unitOfWork.SaveChangesAsync();

                var attachFiles = new List<CollectionEmailFileAttach>();
                if (!string.IsNullOrEmpty(entity.Fk_Collection_Email_File_Attach))
                {
                    var fkAttachIds = entity.Fk_Collection_Email_File_Attach.Split(SpecificSystems.SEMICOLON);
                    foreach (var fkAttachId in fkAttachIds)
                    {
                        attachFiles.Add(new CollectionEmailFileAttach
                        {
                            Fk_Collection_email_Id = collecEmail.Pk_Id,
                            Fk_File_Attach_Id = Convert.ToInt32(fkAttachId)
                        });
                    }
                }
                if (attachFiles.Any())
                {
                    var data = new List<object>();
                    data.AddRange(attachFiles);
                    _genCollecEmailAttachRepository.BulkInsert(data);
                    //_unitOfWork.SaveChanges();
                }

                _unitOfWork.Commit();
            }
            catch (Exception)
            {
                _unitOfWork.Rollback();
                throw;
            }
        }

        public async Task InsertListAsync(List<ViewCollectionEmailDto> entities)
        {
            foreach (var entity in entities)
                await InsertAsync(entity);
        }

        public async Task UpdateAsync(ViewCollectionEmailDto entity)
        {
            try
            {
                _unitOfWork.BeginTransaction();

                var collecEmail = _genCollecEmailRepository.Find(entity.Pk_Id.Value);
                collecEmail.Os_Company = entity.Os_Company;
                collecEmail.Segment = entity.Segment;
                collecEmail.From = entity.From;
                collecEmail.To = entity.To;
                collecEmail.Cc = entity.Cc;
                collecEmail.Bcc = entity.Bcc;
                collecEmail.Subject = entity.Subject;
                collecEmail.Fk_Email_Tempt_Id = entity.Fk_Email_Tempt_Id;
                collecEmail.Recipient_Email = entity.Recipient_Email;
                _genCollecEmailRepository.Update(collecEmail);

                var attachFiles = new List<CollectionEmailFileAttach>();
                if (!string.IsNullOrEmpty(entity.Fk_Collection_Email_File_Attach))
                {
                    var collecEmailAttachs = _collecEmailAttachRepository.FindAllByFkCollecEmailId(entity.Pk_Id.Value);
                    if (collecEmailAttachs != null && collecEmailAttachs.Any())
                    {
                        var data = new List<object>();
                        data.AddRange(collecEmailAttachs);
                        _genCollecEmailAttachRepository.BulkDelete(data);
                    }

                    var fkAttachIds = entity.Fk_Collection_Email_File_Attach.Split(SpecificSystems.SEMICOLON);
                    foreach (var fkAttachId in fkAttachIds)
                    {
                        attachFiles.Add(new CollectionEmailFileAttach
                        {
                            Fk_Collection_email_Id = collecEmail.Pk_Id,
                            Fk_File_Attach_Id = Convert.ToInt32(fkAttachId)
                        });
                    }
                }
                if (attachFiles.Any())
                {
                    var data = new List<object>();
                    data.AddRange(attachFiles);
                    _genCollecEmailAttachRepository.BulkInsert(data);
                }

                await _unitOfWork.SaveChangesAsync();
                _unitOfWork.Commit();
            }
            catch (Exception)
            {
                _unitOfWork.Rollback();
                throw;
            }
        }

        public async Task UpdateListAsync(List<ViewCollectionEmailDto> entities)
        {
            foreach (var entity in entities)
                await UpdateAsync(entity);
        }

        public async Task DeleteAsync(int pk_Id)
        {
            try
            {
                _unitOfWork.BeginTransaction();

                var collecEmailAttachs = _collecEmailAttachRepository.FindAllByFkCollecEmailId(pk_Id);
                if (collecEmailAttachs != null && collecEmailAttachs.Any())
                {
                    var data = new List<object>();
                    data.AddRange(collecEmailAttachs);
                    _genCollecEmailAttachRepository.BulkDelete(data);
                }

                var collecEmail = _genCollecEmailRepository.Find(pk_Id);
                if (collecEmail != null)
                    _genCollecEmailRepository.Delete(collecEmail);

                await _unitOfWork.SaveChangesAsync();
                _unitOfWork.Commit();
            }
            catch (Exception)
            {
                _unitOfWork.Rollback();
                throw;
            }
        }

        public async Task DeleteListAsync(List<int> pk_Ids)
        {
            foreach (var pk_Id in pk_Ids)
                await DeleteAsync(pk_Id);
        }

        public IList<FieldValidateResponse> Validate(ViewCollectionEmailDto entity)
        {
            var results = new List<FieldValidateResponse>();
            try
            {
                if (!string.IsNullOrEmpty(entity.From) && entity.From.Trim().Contains(SpecificSystems.COMMA))
                    results.Add(new FieldValidateResponse
                    {
                        Error = string.Format(ErrorMessages.EM140, nameof(entity.From)),
                        Field = $"{nameof(entity.From)}",
                        Description = string.Format(ErrorMessages.EM140, nameof(entity.From))
                    });

                if (!string.IsNullOrEmpty(entity.To) && entity.To.Trim().Contains(SpecificSystems.COMMA))
                    results.Add(new FieldValidateResponse
                    {
                        Error = string.Format(ErrorMessages.EM140, nameof(entity.To)),
                        Field = $"{nameof(entity.To)}",
                        Description = string.Format(ErrorMessages.EM140, nameof(entity.To))
                    });

                if (!string.IsNullOrEmpty(entity.Cc) && entity.Cc.Trim().Contains(SpecificSystems.COMMA))
                    results.Add(new FieldValidateResponse
                    {
                        Error = string.Format(ErrorMessages.EM140, nameof(entity.Cc)),
                        Field = $"{nameof(entity.Cc)}",
                        Description = string.Format(ErrorMessages.EM140, nameof(entity.Cc))
                    });

                if (!string.IsNullOrEmpty(entity.Bcc) && entity.Bcc.Trim().Contains(SpecificSystems.COMMA))
                    results.Add(new FieldValidateResponse
                    {
                        Error = string.Format(ErrorMessages.EM140, nameof(entity.Bcc)),
                        Field = $"{nameof(entity.Bcc)}",
                        Description = string.Format(ErrorMessages.EM140, nameof(entity.Bcc))
                    });

                if (!string.IsNullOrEmpty(entity.Recipient_Email) && entity.Recipient_Email.Trim().Contains(SpecificSystems.COMMA))
                    results.Add(new FieldValidateResponse
                    {
                        Error = string.Format(ErrorMessages.EM140, nameof(entity.Recipient_Email)),
                        Field = $"{nameof(entity.Recipient_Email)}",
                        Description = string.Format(ErrorMessages.EM140, nameof(entity.Recipient_Email))
                    });
                foreach (var to in entity.To.Split(SpecificCharacteristics.SEMICOLON).ToList())
                {
                    if (!EmailValidate.IsValid(to))
                    {
                        results.Add(new FieldValidateResponse
                        {
                            Error = string.Format(ErrorMessages.EM141, nameof(entity.To)),
                            Field = $"{nameof(entity.To)}",
                            Description = string.Format(ErrorMessages.EM141, nameof(entity.To))
                        });
                        break;
                    }
                }
                foreach (var cc in entity.Cc.Split(SpecificCharacteristics.SEMICOLON).ToList())
                {
                    if (!EmailValidate.IsValid(cc))
                    {
                        results.Add(new FieldValidateResponse
                        {
                            Error = string.Format(ErrorMessages.EM141, nameof(entity.Cc)),
                            Field = $"{nameof(entity.Cc)}",
                            Description = string.Format(ErrorMessages.EM141, nameof(entity.Cc))
                        });
                        break;
                    }
                }
                if (!string.IsNullOrEmpty(entity.Bcc))
                {
                    foreach (var bcc in entity.Bcc.Split(SpecificCharacteristics.SEMICOLON).ToList())
                    {
                        if (!EmailValidate.IsValid(bcc))
                        {
                            results.Add(new FieldValidateResponse
                            {
                                Error = string.Format(ErrorMessages.EM141, nameof(entity.Bcc)),
                                Field = $"{nameof(entity.Bcc)}",
                                Description = string.Format(ErrorMessages.EM141, nameof(entity.Bcc))
                            });
                            break;
                        }
                    }
                }

                foreach (var recipient in entity.Recipient_Email.Split(SpecificCharacteristics.SEMICOLON).ToList())
                {
                    if (!EmailValidate.IsValid(recipient))
                    {
                        results.Add(new FieldValidateResponse
                        {
                            Error = string.Format(ErrorMessages.EM141, nameof(entity.Recipient_Email)),
                            Field = $"{nameof(entity.Recipient_Email)}",
                            Description = string.Format(ErrorMessages.EM141, nameof(entity.Recipient_Email))
                        });
                        break;
                    }
                }

                if (!entity.Pk_Id.HasValue || entity.Pk_Id == 0)
                {
                    //Valid case insert
                    if (_genCollecEmailRepository.Queryable().AsEnumerable().FirstOrDefault(c => entity.Os_Company.Equals(c.Os_Company, StringComparison.CurrentCultureIgnoreCase) && entity.Segment.Equals(c.Segment, StringComparison.CurrentCultureIgnoreCase)) != null)
                    {
                        results.Add(new FieldValidateResponse
                        {
                            Error = ErrorMessages.EM139,
                            Field = $"{nameof(entity.Os_Company)},{nameof(entity.Segment)}",
                            Description = ErrorMessages.EM139
                        });
                    }

                }
                else
                {
                    //valid case update
                    if (_genCollecEmailRepository.Queryable().AsEnumerable().FirstOrDefault(c => entity.Os_Company.Equals(c.Os_Company, StringComparison.CurrentCultureIgnoreCase) && entity.Segment.Equals(c.Segment, StringComparison.CurrentCultureIgnoreCase) && c.Pk_Id != entity.Pk_Id) != null)
                    {
                        results.Add(new FieldValidateResponse
                        {
                            Error = ErrorMessages.EM139,
                            Field = $"{nameof(entity.Os_Company)},{nameof(entity.Segment)}",
                            Description = ErrorMessages.EM139
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                results.Add(new FieldValidateResponse
                {
                    Error = ex.Message,
                    Field = Constants.BUSSINESS_EXCEPTION,
                    Description = ex.Message
                });
            }
            return results;
        }
    }
}
