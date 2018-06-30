namespace QSP.GoogleMap
{
    public static class StaticMap
    {

        public static string GetMapUrl(double lat, double lon, int imageWidth, int imageHeight)
        {
            // Use scale of 2 instead to get larger map.
            // Otherwise API limits the map size to 640 * 640.
            //
            imageWidth /= 2;
            imageHeight /= 2;

            return @"https://maps.googleapis.com/maps/api/staticmap?center=" +
                   $"{lat},{lon}&zoom=13&size={imageWidth}x{imageHeight}&scale=2&maptype=satellite";
        }
    }
}
