using UnityEngine;
using System.Collections;

public class BattleConfig {

	//layer
	public enum LAYER
	{
		NONE,
		TANK,
		TRACK,
		BULLET,
		EFFECT,
		OTHER,
	}
	public static string GetLayerName(LAYER layer)
	{
		switch (layer) {
		case LAYER.TANK: return "LayerTank";
		case LAYER.TRACK: return "LayerTrack";
		case LAYER.BULLET: return "LayerBullet";
		case LAYER.EFFECT: return "LayerEffect";
		case LAYER.OTHER: return "LayerOther";
		}
		
		return null;
	}


	public const bool DEAD_UNIT_COLLISION = false;


}
