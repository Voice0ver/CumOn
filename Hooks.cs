using System;
using HarmonyLib;

namespace CumOn
{
	public partial class CumOn
	{
		public static HScene hScene;
		public static HSceneFlagCtrl hFlagCtrl;

		private static bool paramsSet = false; // Flags whether the fluid parameters for the current scene have been overwritten yet
		private static int shotNumber;
		private static float currTime;
		private static float currVolumeTime;
		private static float currShotTime;
		private static float currShotSpeed;
		private static Random rand = new Random();


		[HarmonyPostfix, HarmonyPatch(typeof(HScene), "LateUpdate")]
		public static void Prefix_HScene_LateUpdate(HScene __instance)
		{
			hScene = __instance;
			hFlagCtrl = hScene.ctrlFlag;

			if (hFlagCtrl.nowOrgasm)
            {
				// Initial parameter setup for all fluids occurs when orgasm is triggered
				if (!paramsSet)
				{
					//----Cumshots----

					var emitterCtrl = hScene.ctrlObi.obiFluidCtrlMale[0].ObiEmitterCtrls[0]; //TODO!!!!!!!!!!
					// Ignore scenes where there isn't a cumshot
					if (emitterCtrl.splitInfos.Count == 0 || emitterCtrl.splitInfos[0].Params.Length == 0)
					{
					}
					else
					{
						// The splitInfos array contains details of cumshot timings, but since the timing changes here are complex it
						// is easier to ignore that array for cumshots and control the emitter on/off logic directly (later in code)

						// Particle system settings
						var emitter = emitterCtrl.ObiEmitter;
						emitter.NumParticles = NumParticles.Value;
						emitter.lifespan = ParticleLife.Value;

						// Fluid settings
						var emitterMaterial = (Obi.ObiEmitterMaterialFluid)emitter.EmitterMaterial;
						if (Resolution.Value != 0) emitterMaterial.resolution = Resolution.Value;
						emitterMaterial.buoyancy = -GravityMultiplier.Value; // Changing effective gravity on fluids by adjusting buoyancy rather than adjusting gravity itself
					}

					//----Cum Vomit----

					// Allow few user changes for cum vomit since settings need to be precise for it to flow out of the mouth
					var vomitEmitterCtrl = hScene.ctrlObi.obiFluidCtrlFemale[0].ObiEmitterCtrls[2]; //TODO!!!!!!!!!!
					// Ignore scenes where there isn't cum vomit
					if (vomitEmitterCtrl.splitInfos.Count == 0 || vomitEmitterCtrl.splitInfos[0].Params.Length == 0)
					{
					}
					else
					{
						// No need for complex timings as with cumshots so simply update the game's cum values - 'Rate' really means volume
						vomitEmitterCtrl.splitInfos[0].Params[0].Rate = VomitAmount.Value;

						// Particle system and material settings
						var vomitEmitter = vomitEmitterCtrl.ObiEmitter;
						vomitEmitter.NumParticles = NumParticles.Value;
						var vomitEmitterMaterial = (Obi.ObiEmitterMaterialFluid)vomitEmitter.EmitterMaterial;
						vomitEmitterMaterial.buoyancy = -2;  // This setting in particular is needed for the flow to work with different amounts
					}

					paramsSet = true;
					shotNumber = 0;
				}
			}
			else
			{
				// Disable plugin code only *after* the orgasm flag has been cleared to avoid reentry into block above
				// However, as we allow cum effects into the post-cum scene only clear this flag when cumshots are done
				if (shotNumber > NumShots.Value)  paramsSet = false;
			}

			if (paramsSet && shotNumber <= NumShots.Value)
			{
				var emitterCtrl = hScene.ctrlObi.obiFluidCtrlMale[0].ObiEmitterCtrls[0]; //TODO!!!!!!!!!!
				var emitter = emitterCtrl.ObiEmitter;
				if (shotNumber == 0)
                {
					if (emitter.playMode == Obi.ObiEmitter.PlayMode.Play)
					{
						shotNumber = 1;
						currShotTime = ShotTime.Value;
						currVolumeTime = ShotTime.Value * ShotVolume.Value;
						currShotSpeed = ShotSpeed.Value;
						emitter.speed = currShotSpeed;
						emitter.randomVelocity = Randomness.Value / emitter.speed;
						currTime = 0;
					}
				}
				else
                {
					if (currTime < currShotTime)
					{
						// Exponential decay (kinda) looks better than linear. Testing futher tweak to the curve.
						float decay = 1 - (float)Math.Pow(1 - ShotStretch.Value, Test.Value / currVolumeTime);
						emitter.speed = emitter.speed * (float)Math.Pow(1 - decay, UnityEngine.Time.unscaledDeltaTime);
					}
					else
					{
						do
						{
							++shotNumber;
							currShotTime *= 1 + ShotTimeIncrease.Value;
							currVolumeTime *= 1 - ShotVolumeDecrease.Value;
							currShotSpeed *= 1 - ShotSpeedDecrease.Value;
							currTime -= currShotTime;
						} while (currTime >= currShotTime);
						emitter.speed = currShotSpeed;
					}
					if (emitter.speed > 0) emitter.randomVelocity = Randomness.Value / emitter.speed;
					if (RandomiseLife.Value)
					{
						emitter.lifespan = 1 + 2 * (ParticleLife.Value - 1) * (float)rand.NextDouble(); // Random value centred on user value (1.0 upwards)
					}
					else
					{
						emitter.lifespan = ParticleLife.Value;
					}

					currTime += UnityEngine.Time.unscaledDeltaTime;
				}
				emitter.playMode = (shotNumber >= 1 && shotNumber <= NumShots.Value && currTime < currVolumeTime) ? Obi.ObiEmitter.PlayMode.Play : Obi.ObiEmitter.PlayMode.Stop;
			}
		}


		[HarmonyPrefix, HarmonyPatch(typeof(CrossFade), "FadeStart")]
		public static void Prefix_Crossfade_FadeStart(CrossFade __instance, ref float time)
		{
			if (!Crossfades.Value)  time = 0.001f;
		}
	}
}
