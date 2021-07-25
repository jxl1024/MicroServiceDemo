using MicroService.Model;

namespace MicroService.TeamService.Models
{
    /// <summary>
    /// 团队实体
    /// </summary>
    public class Team : BaseModel
    {
        /// <summary>
        /// 团队名称
        /// </summary>
        public string Name { set; get; }
    }
}
