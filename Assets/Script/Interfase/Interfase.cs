using System.Collections;
using System.Collections.Generic;
using YiFramework.Setting;
using UnityEngine;

namespace YiFramework.Interfase
{
    #region 事件接口，继承这两个接口就可以发送消息事件（使用扩展方法发送和注册消息）

    public interface ISendEvent
    {
        /// <summary>
        /// 作为发送者的类型
        /// </summary>
        public ESendType SendType { get; }
    }

    public interface IReceiveEvent
    {
        /// <summary>
        /// 作为接受者的类型
        /// </summary>
        public EReceiveType ReceiveType { get; }
    }

    #endregion
    
    #region 单例接口

    public interface ISingleMonoInterface
    {
    }

    public interface ISingleInterface
    {
    }

    #endregion
}
