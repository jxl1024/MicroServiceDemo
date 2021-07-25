using System;

namespace MicroService.Model
{
    public class BaseModel
    {
        /// <summary>
        /// 主键
        /// </summary>
        public int ClusterID { get; set; }

        public Guid ID { get; set; } = Guid.NewGuid();

        public string CreateUser { get; set; }

        public DateTime CreateTime { get; set; } = DateTime.Now;

        public string UpdateUser { get; set; }

        public DateTime UpdateTime { get; set; } = DateTime.Now;
    }
}
