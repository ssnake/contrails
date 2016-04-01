using UnityEngine;
using System.Collections;
using System;

public abstract class CoordConverter
{
    public abstract void LatLong2XY(float latitude, float longitude, out float x, out float y);
    public abstract float GetScale(float latitude=0.0f);
}
public class Mercator1 : CoordConverter
{
    float R = 6371000f;
    float mapWidth;
    public Mercator1(int mapWidth)
    {
        this.mapWidth = mapWidth;
    }

    public override float GetScale(float latitude)
    {
        return R / mapWidth;
    }

    public override void LatLong2XY(float latitude, float longitude, out float x, out float y)
    {
        x = (float )(mapWidth * longitude / 2.0f/180.0f) ;
        y = (float) (mapWidth  / 2.0f / 180.0f * Math.Log(Math.Tan(45 + latitude / 2)));
    }
    
}
public class WebMercator1 : CoordConverter
{
    float R = 6378137.0f;
    int level;
    int mapWidthBase;
    int screenDPI = 96;
    public WebMercator1(int mapWidthBase, int level)
    {
        this.mapWidthBase = mapWidthBase;
        this.level = level;
    }

    float GetMapWidth()
    {
        return (float) (mapWidthBase * Math.Pow(2.0f, (float) level));
    }
   
    public override float GetScale(float latitude)
    {
        float groundRes = (float) (Math.Cos(latitude * Math.PI / 180.0f) * R / GetMapWidth());
        return 1.0f / ((groundRes * screenDPI)/(GetMapWidth()*0.0254f));
    }

    public override void LatLong2XY(float latitude, float longitude, out float x, out float y)
    {
        x = (float)(GetMapWidth() * (longitude + 180.0f) / 360.0f);
        float sinLat = (float)Math.Sin(latitude * Math.PI / 180);
        y = (float)((0.5 - Math.Log((1+sinLat)/ (1-sinLat))/(4*Math.PI))* GetMapWidth() );
    }
}
public class MapController {
    CoordConverter converter;
    public MapController()
    {
        converter = new WebMercator1(256, 6);

    }
    public void LatLong2XY(float latitude, float longitude, float altitude,  out float x, out float y, out float alt)
    {
        converter.LatLong2XY(latitude, longitude, out x, out y);
        var scale = converter.GetScale(latitude);
        alt = altitude * scale;

    }
}
