using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Model_shop {

	private PBConnect_buyShopItem.SHOP_TYPE _shopType = PBConnect_buyShopItem.SHOP_TYPE.UNKNOWN;
	public PBConnect_buyShopItem.SHOP_TYPE shopType
	{
		get { return _shopType; }
	}

	private List<SlgPB.ShopItem> _shopItems = new List<SlgPB.ShopItem>();
	public List<SlgPB.ShopItem> shopItems
	{
		get { return _shopItems; }
	}

	public SlgPB.ShopItem GetItem(int index)
	{
		return shopItems [index];
	}

	public void Update(PBConnect_buyShopItem.SHOP_TYPE shopType, List<SlgPB.ShopItem> shopItems)
	{
		_shopType = shopType;
		_shopItems = shopItems;
	}

}
