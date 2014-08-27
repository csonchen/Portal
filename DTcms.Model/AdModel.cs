using System;
using System.Collections.Generic;
using System.Text;

namespace DTcms.Model
{
    #region## Ad域信息实体
    /// <summary>
    /// Ad域信息实体
    /// </summary>
    public class AdModel
    {
        public AdModel(string id, string name, int typeId, string parentId,string displayName,string job)
        {
            Id = id;
            Name = name;
            TypeId = typeId;
            ParentId = parentId;
            DisplayName = displayName;
            Job = job;
        }

        public AdModel(string id, string name, int typeId, string parentId)
        {
            Id = id;
            Name = name;
            TypeId = typeId;
            ParentId = parentId;
        }

        public AdModel()
        {
            // TODO: Complete member initialization
        }

        public string Job { get; set; }

        public string DisplayName { get; set; }

        public string Id { get; set; }

        public string Name { get; set; }

        public int TypeId { get; set; }

        public string ParentId { get; set; }
    }
    #endregion
}
