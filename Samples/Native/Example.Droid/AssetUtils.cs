using System;
using System.Collections.Generic;
using System.Linq;
using Android.Content;

namespace LottieSamples.Droid
{
    public static class AssetUtils
    {
        public static IList<string> GetJsonAssets(Context context, string path)
        {
            var assetList = context.Assets.List(path);
            return assetList.Where(item => item.EndsWith(".json", StringComparison.InvariantCultureIgnoreCase)).ToList();
        }
    }
}
