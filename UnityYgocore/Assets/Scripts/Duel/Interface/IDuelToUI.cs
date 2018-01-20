using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// 让ui调用的接口
/// </summary>
public interface IDuelToUI
{
    /// <summary>
    /// 更新选择卡片的信息显示 为卡牌调用
    /// </summary>
    /// <param name="id"></param>
    void UpdateSelectCardShow(string id);

    /// <summary>
    /// 操作卡片 为选项列表所调用
    /// </summary>
    /// <param name="targetArea"></param>
    /// <param name="targetRank"></param>
    /// <param name="str"></param>
    void OperateCard(int targetArea,int targetRank,string str,bool isMy);

    /// <summary>
    /// 获取卡片的操作列表 为为选项列表所调用
    /// </summary>
    /// <param name="targetArea"></param>
    /// <param name="targetRank"></param>
    /// <returns></returns>
    List<string> GetCardOption(int targetArea, int targetRank, bool isMy);

    void ChangeToNextPhaseDuel(int phase);
}

