using BepInEx;
using BepInEx.Configuration;

namespace CumOn
{
	[BepInProcess("HoneySelect2")]
	[BepInPlugin(GUID, Name, Version)]
	public partial class CumOn : BaseUnityPlugin
	{
		const string GUID = "Voice0ver.CumOn";
		const string Name = "CumOn";
		const string Version = "0.1.0";

		const string SECTION_CUM = "Cumshots";
		const string SECTION_FLUID = "Further Fluids";
		const string SECTION_SIM = "Simulation";
		const string SECTION_MISC = "Miscellaneous";

		const string DESCRIPTION_NUMSHOTS =
			"How many cum shots in a single scene";
		const string DESCRIPTION_SHOTTIME =
			"How long in seconds from one shot to the next";
		const string DESCRIPTION_SHOTTIMEINCREASE =
			"How much the time from one shot to the next increases each time";
		const string DESCRIPTION_SHOTSPEED =
			"How fast the cum flies (first shot)";
		const string DESCRIPTION_SHOTSPEEDDECREASE =
			"How much the cum speed decreases with each shot";
		const string DESCRIPTION_SHOTVOLUME =
			"How much volume of comes out (first shot)";
		const string DESCRIPTION_SHOTVOLUMEDECREASE =
			"Decrease in cum volume from shot to shot";
		const string DESCRIPTION_SHOTTAIL =
			"How much to stretch each shot out as it flies";
		const string DESCRIPTION_RANDOMNESS =
			"A level of randomness to the emission direction, low value = jet, high value = spray";
		const string DESCRIPTION_VOMITAMOUNT =
			"Amount of cum vomit the girl brings up";
		const string DESCRIPTION_GRAVITY =
			"Gravity adjustment from the default. Higher values more realistic, but the fluid system visuals can't keep up";
		const string DESCRIPTION_RESOLUTION =
			"Integer. Higher values use smaller particles for finer detail but slower speed. 1 works often and is fastest, try 2, 3 or 4. Set 0 to use the HS2 default value.";
		const string DESCRIPTION_PARTICLELIFE =
			"How long each particle lasts before it disappears. Decrease for performance (but no residue)";
		const string DESCRIPTION_RANDOMISELIFE =
			"Randomises the particle life values a little for slightly better visuals, leave on in general";
		const string DESCRIPTION_NUMPARTICLES =
			"Leave this alone for now";
		const string DESCRIPTION_CROSSFADE =
			"Enable/disable all crossfades";

		internal static ConfigEntry<int> NumShots { get; set; }
		internal static ConfigEntry<float> ShotTime { get; set; }
		internal static ConfigEntry<float> ShotTimeIncrease { get; set; }
		internal static ConfigEntry<float> ShotSpeed { get; set; }
		internal static ConfigEntry<float> ShotSpeedDecrease { get; set; }
		internal static ConfigEntry<float> ShotVolume { get; set; }
		internal static ConfigEntry<float> ShotVolumeDecrease { get; set; }
		internal static ConfigEntry<float> ShotStretch { get; set; }
		internal static ConfigEntry<float> Randomness { get; set; }
		internal static ConfigEntry<float> GravityMultiplier { get; set; }
		internal static ConfigEntry<int> Resolution { get; set; }
		internal static ConfigEntry<int> NumParticles { get; set; }
		internal static ConfigEntry<float> ParticleLife { get; set; }
		internal static ConfigEntry<bool> RandomiseLife { get; set; }
		internal static ConfigEntry<float> VomitAmount { get; set; }
		internal static ConfigEntry<float> Test { get; set; }
		internal static ConfigEntry<bool> Crossfades { get; set; }

		internal void Awake()
		{
			NumShots           = Config.Bind(SECTION_CUM,   "1. Number of shots",            5,     DESCRIPTION_NUMSHOTS);
			ShotTime           = Config.Bind(SECTION_CUM,   "2. Time from shot to shot",     0.7f,  DESCRIPTION_SHOTTIME);
			ShotTimeIncrease   = Config.Bind(SECTION_CUM,   "3. Shot to shot time slowdown", 0.1f,  new ConfigDescription(DESCRIPTION_SHOTTIMEINCREASE,   new AcceptableValueRange<float>(0, 1)));
			ShotSpeed          = Config.Bind(SECTION_CUM,   "4. Shot speed",                 18.0f, DESCRIPTION_SHOTSPEED);
			ShotSpeedDecrease  = Config.Bind(SECTION_CUM,   "5. Shot speed decrease",        0.15f, new ConfigDescription(DESCRIPTION_SHOTSPEEDDECREASE,  new AcceptableValueRange<float>(0, 1)));
			ShotVolume         = Config.Bind(SECTION_CUM,   "6. Shot Volume",                0.40f, new ConfigDescription(DESCRIPTION_SHOTVOLUME,         new AcceptableValueRange<float>(0, 1))); ;
			ShotVolumeDecrease = Config.Bind(SECTION_CUM,   "7. Shot volume decrease",       0.2f,  new ConfigDescription(DESCRIPTION_SHOTVOLUMEDECREASE, new AcceptableValueRange<float>(0, 1)));
			ShotStretch        = Config.Bind(SECTION_CUM,   "8. Shot tail",                  0.75f, new ConfigDescription(DESCRIPTION_SHOTTAIL,        new AcceptableValueRange<float>(0, 1)));
			Randomness         = Config.Bind(SECTION_CUM,   "9. Randomness of the spray",    3.0f,  DESCRIPTION_RANDOMNESS);
			VomitAmount        = Config.Bind(SECTION_FLUID, "1. How much cum to vomit",      4.0f,  DESCRIPTION_VOMITAMOUNT);
			GravityMultiplier  = Config.Bind(SECTION_SIM,   "1. Gravity multiplier",         2.0f,  DESCRIPTION_GRAVITY);
			Resolution         = Config.Bind(SECTION_SIM,   "2. Simulation resolution",      1,     DESCRIPTION_RESOLUTION);
			ParticleLife       = Config.Bind(SECTION_SIM,   "3. Lifetime of each particle",  3.0f,  DESCRIPTION_PARTICLELIFE);
			RandomiseLife      = Config.Bind(SECTION_SIM,   "4. Randomise Lifetimes",        true,  DESCRIPTION_RANDOMISELIFE);
			NumParticles       = Config.Bind(SECTION_SIM,   "5. Number of Particles (max)",  1600,  DESCRIPTION_NUMPARTICLES);
			Crossfades         = Config.Bind(SECTION_MISC,  "1. H-Scene Crossfades",         false, DESCRIPTION_CROSSFADE);
			Test = Config.Bind(SECTION_MISC, "2.Test", 0.5f, "");

			HarmonyLib.Harmony.CreateAndPatchAll(typeof(CumOn));
		}

		internal void Update()
		{
			Controller.Update();
		}
	}
}
