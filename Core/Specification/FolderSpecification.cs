using System;
using Core.Entities;

namespace Core.Specification;

public class FolderSpecification : BaseSpecification<Folder>
{
    public FolderSpecification(string userId) : base(x => x.AppUserId == userId)
    {
        
    }

    public FolderSpecification(string userId, int folder_id) : base(x => x.AppUserId == userId  && x.Id == folder_id)
    {
        
    }
}
