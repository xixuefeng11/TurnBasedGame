﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace GameState
{
	public class StartNormal : Start.Sub
	{

		public VP<float> time;

		public VP<float> duration;

		#region Constructor

		public enum Property
		{
			time,
			duration
		}

		public StartNormal() : base()
		{
			this.time = new VP<float> (this, (byte)Property.time, 0);
            this.duration = new VP<float>(this, (byte)Property.duration, Setting.get().fastStart.v ? 0 : 3);
		}

		#endregion

		public override Type getType ()
		{
			return Type.Normal;
		}

	}
}