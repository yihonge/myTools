using System;

namespace myTools
{
    public class LocalTVLive
    {
        /// <summary>
        /// 节目名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 节目路径
        /// </summary>
        public string Path { get; set; }
    }

    public class ServerTVLive
    {
        /// <summary>
        /// 节目编号
        /// </summary>
        public int ProgrammeID { get; set; }
        /// <summary>
        /// 节目名称
        /// </summary>
        public string ProgrammeName { get; set; }
        /// <summary>
        /// 节目路径
        /// </summary>
        public string ProgrammePath { get; set; }
        /// <summary>
        /// 节目分类
        /// </summary>
        public int ProgrammeGroup { get; set; }
        /// <summary>
        /// 节目省份
        /// </summary>
        public int ProgrammeProvince { get; set; }
        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime UpdateTime { get; set; }
    }
}
