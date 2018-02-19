using System;
using System.Collections.Generic;
using Android.Content;
using System.Linq;

namespace LottieSamples.Droid
{
    public static class AssetUtils
    {
        public static IList<string> GetJsonAssets(Context context, string path)
        {
            string [] assetList = context.Assets.List(path);
            return assetList.Where(item => item.EndsWith(".json", StringComparison.InvariantCultureIgnoreCase)).ToList();
        }
    }
}
