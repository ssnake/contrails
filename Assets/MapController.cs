using UnityEngine;
using System.Collections;
using System;

public abstract class CoordConverter
{
    public abstract void LatLong2XY(float latitude, float longitude,float alitutde, out float x, out float y, out float alt);
    public abstract float GetScale(float latitude=0.0f);
}
public class Mercator1 : CoordConverter
{
    float R = 6378137.0f;
    float mapWidth;
    public Mercator1(float mapWidth)
    {
        this.mapWidth = mapWidth;
    }

    public override float GetScale(float latitude)
    {
        return (float) (2*Math.PI * R / mapWidth)
            ;
    }

    public override void LatLong2XY(float latitude, float longitude, float altitude, out float x, out float y, out float alt)
    {
        x = (float )(mapWidth * longitude / 2.0f/180.0f) ;
        y = (float) (mapWidth  / 2.0f / 180.0f * Math.Log(Math.Tan(Math.PI/4.0 + latitude / 2)));
        alt = GetScale(latitude) * altitude;

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

    public override void LatLong2XY(float latitude, float longitude, float altitude, out float x, out float y, out float alt)
    {
        x = (float)(GetMapWidth() * (longitude + 180.0f) / 360.0f);
        float sinLat = (float)Math.Sin(latitude * Math.PI / 180);
        y = (float)((0.5 - Math.Log((1+sinLat)/ (1-sinLat))/(4*Math.PI))* GetMapWidth() );
        alt = GetScale(latitude) * altitude;
    }
}

public class SphereMap
{
    
    float radius;
    public SphereMap(float radiusM)
    {
        this.radius = radiusM;
    

    }

    public void Adjust(ref float x, ref float y, ref float alt)
    {
        if (Vector3.Distance(new Vector3(x, alt, y), Vector3.zero) < radius)
            return;
        if ((x != 0.0) && (y != 0.0))
        { 
            var alpha = Math.Atan(y / x);
            var OA1 = y / Math.Sin(alpha);
            var beta = Math.Atan(alt / OA1);
            alt = (float)(radius * Math.Sin(beta));
            var OB1 = radius * Math.Cos(beta);
            x = (float)(OB1 * Math.Cos(alpha));
            y = (float)(OB1 * Math.Sin(alpha));
        } else
        {
            alt = radius;
        }


    }
}
public class MapController {
    CoordConverter converter;
    public MapController()
    {
        //converter = new WebMercator1(256, 9);
        converter = new Mercator1((float) (2*Math.PI*6378137));
        

    }
    public void LatLong2XY(float latitude, float longitude, float altitude,  out float x, out float y, out float alt)
    {
        converter.LatLong2XY(latitude, longitude, altitude,  out x, out y, out alt);
        

    }
}
