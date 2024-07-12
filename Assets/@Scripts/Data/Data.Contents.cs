using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;

namespace Data
{
    ////Json을 활용하는경우
    //#region PlayerDataJson
    //[Serializable]
    //public class PlayerData
    //{
    //    public int level;
    //    public int maxHp;
    //    public int attack;
    //    public int totalExp;
    //}

    //[Serializable]
    //public class PlayerDataLoader : ILoader<int, PlayerData>
    //{
    //    public List<PlayerData> stats = new List<PlayerData>();

    //    public Dictionary<int, PlayerData> MakeDict()
    //    {
    //        Dictionary<int, PlayerData> dict = new Dictionary<int, PlayerData>();
    //        foreach (PlayerData stat in stats)
    //        {
    //            dict.Add(stat.level, stat);
    //        }
    //        return dict;
    //    }
    //}
    //#endregion

    //Json을 활용하는경우
    #region PlayerDataXml
    public class PlayerData
    {
        [XmlAttribute]
        public int level;
        [XmlAttribute]
        public int maxHp;
        [XmlAttribute]
        public int attack;
        [XmlAttribute]
        public int totalExp;
    }

    [Serializable, XmlRoot("PlayerDatas")]
    public class PlayerDataLoader : ILoader<int, PlayerData>
    {
        [XmlElement("PlayerData")]
        public List<PlayerData> stats = new List<PlayerData>();
        public Dictionary<int, PlayerData> MakeDict()
        {
            Dictionary<int, PlayerData> dict = new Dictionary<int, PlayerData>();
            foreach(PlayerData stat in stats)
            {
                dict.Add(stat.level, stat);
            }
            return dict;            
        }
    }
	#endregion

	#region MonsterData

	public class MonsterData
	{
		[XmlAttribute]
		public string name;
		[XmlAttribute]
		public string prefab;
		[XmlAttribute]
		public int level;
		[XmlAttribute]
		public int maxHp;
		[XmlAttribute]
		public int attack;
		[XmlAttribute]
		public float speed;
		// DropData
		// -      Ȯ    
		// -            (    ,   ų     ,    ,    )
		// -              ?


	}

	#endregion

	#region SkillData

	[Serializable]
	public class HitEffect
	{
		[XmlAttribute]
		public string type;
		[XmlAttribute]
		public int templateID;
		[XmlAttribute]
		public int value;
	}

	public class SkillData
	{
		[XmlAttribute]
		public int templateID;

		[XmlAttribute(AttributeName = "type")]
		//public string skillTypeStr;
		public Define.SkillType skillType = Define.SkillType.None;

		[XmlAttribute]
		public int nextID;
		public int prevID = 0; // TODO

		[XmlAttribute]
		public string prefab;

		//          
		[XmlAttribute]
		public int damage;
		//[XmlElement("HitEffect")]
		//public List<HitEffect> hitEffects = new List<HitEffect>();
	}

	[Serializable, XmlRoot("SkillDatas")]
	public class SkillDataLoader : ILoader<int, SkillData>
	{
		[XmlElement("SkillData")]
		public List<SkillData> skills = new List<SkillData>();

		public Dictionary<int, SkillData> MakeDict()
		{
			Dictionary<int, SkillData> dict = new Dictionary<int, SkillData>();
			foreach (SkillData skill in skills)
				dict.Add(skill.templateID, skill);

			return dict;
		}
	}

	#endregion

}
