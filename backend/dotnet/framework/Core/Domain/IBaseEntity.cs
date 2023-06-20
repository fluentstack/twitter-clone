namespace FSH.Framework.Core.Domain;

public interface IBaseEntity<TId>
{
    TId Id { get; }
    string? CreatedBy { get; }
    DateTime? LastModifiedOn { get; }
    string? LastModifiedBy { get; }
    bool IsDeleted { get; }
    void UpdateIsDeleted(bool isDeleted);
    void UpdateModifiedProperties(DateTime? lastModifiedOn, string lastModifiedBy);
}