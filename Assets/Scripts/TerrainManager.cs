using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TerrainTools;

public class TerrainManager : MonoBehaviour
{
    private Terrain _terrain;
    public Texture2D BrushTexture;
    public Texture2D BrushTextureHump;
    private TerrainData _terrainData;
    private TerrainBrush _terrainBrush;
    public float k = 0.1f;
    public float humpHeight = 1f;
    
    private void Start(){
        _terrain = GetComponent<Terrain>();
        _terrainData = _terrain.terrainData;
        _terrainBrush = new TerrainBrush(BrushTexture, BrushTextureHump);
        
    }
    public void CreateCrater(Vector3 position, float radius)
    {
        int res = _terrainData.heightmapResolution;
        int xStart = (int)((position.x - radius) / _terrainData.size.x * res);
        int yStart = (int)((position.z - radius) / _terrainData.size.z * res);
        
        int width = (int)((position.x + radius) / _terrainData.size.x * res) - xStart;
        
        
        
        int posOnTerrainX = (int)(position.x / _terrainData.size.x * res);
        int posOnTerrainY = (int)(position.z / _terrainData.size.z * res);
        
        float radiusInTerrain = radius / _terrainData.size.x * res;
        
        float [,] heights = _terrainData.GetHeights(xStart, yStart, width, width);
        float [,] brushHeights = _terrainBrush.GetHeights(width, humpHeight);
        
        float posOnTerrainZ = (position.y - heights[width/2, width/2] * res);
        
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < width; j++)
            {
                //heights[i, j] -= brushHeights[i, j] * k;
                float distance = Vector2.Distance(new Vector3(i, 0, j), new Vector3(posOnTerrainX - xStart, posOnTerrainZ, posOnTerrainY - yStart));
                if (distance < radiusInTerrain)
                {
                    float delta = 1 - distance / radiusInTerrain;
                    heights[i, j] -= delta * brushHeights[i, j] * k;
                }
            }
        }
        
        _terrainData.SetHeights(xStart, yStart, heights);
    }

    public class TerrainBrush
    {
        private Texture2D brushTextureBase;
        private Texture2D brushTextureHump;
        private float[,] _brushHeights;
        
        public TerrainBrush(Texture2D brushTextureBase, Texture2D brushTextureHump)
        {
            this.brushTextureBase = brushTextureBase;
            this.brushTextureHump = brushTextureHump;
        }
        
        public float[,] GetHeights(int width, float humpHeight) 
        {
            // звичайне розширення текстури 512x512, треба зменшини її до значення widthxwidth
            Texture2D brushTexture = new Texture2D(width, width);
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    brushTexture.SetPixel(i, j, 
                        brushTextureBase.GetPixel(
                            i * brushTextureBase.width / width, 
                            j * brushTextureBase.height / width) 
                        );
                }
            }
            Texture2D brushTextureHump = new Texture2D(width, width);
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    brushTextureHump.SetPixel(i, j, 
                        brushTextureHump.GetPixel(
                            i * brushTextureHump.width / width, 
                            j * brushTextureHump.height / width) 
                    );
                }
            }
            brushTexture.Apply();
            _brushHeights = new float[width, width];
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    _brushHeights[i, j] = brushTexture.GetPixel(i, j).grayscale - brushTextureHump.GetPixel(i, j).grayscale * humpHeight;
                }
            }
            return _brushHeights;
            
        }
        
    }
}
