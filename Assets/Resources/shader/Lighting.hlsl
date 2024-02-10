//    Copyright (C) 2020 NedMakesGames

//    This program is free software: you can redistribute it and/or modify
//    it under the terms of the GNU General Public License as published by
//    the Free Software Foundation, either version 3 of the License, or
//    (at your option) any later version.

//    This program is distributed in the hope that it will be useful,
//    but WITHOUT ANY WARRANTY; without even the implied warranty of
//    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
//    GNU General Public License for more details.

//    You should have received a copy of the GNU General Public License
//    along with this program. If not, see <https://www.gnu.org/licenses/>.

// This is the Lighting.hlsl file for the end of the first video in 2020.2

#ifndef CUSTOM_LIGHTING_INCLUDED
#define CUSTOM_LIGHTING_INCLUDED

void CalculateMainLight_float(float3 WorldPos, out float3 Direction, out float3 Color, out half DistanceAtten, out half ShadowAtten) {
#ifdef SHADERGRAPH_PREVIEW
    Direction = half3(0.5, 0.5, 0);
    Color = 1;
    DistanceAtten = 1;
    ShadowAtten = 1;
#else
#if SHADOWS_SCREEN
    half4 clipPos = TransformWorldToHClip(WorldPos);
    half4 shadowCoord = ComputeScreenPos(clipPos);
#else
    half4 shadowCoord = TransformWorldToShadowCoord(WorldPos);
#endif
    Light mainLight = GetMainLight(shadowCoord);
    Direction = mainLight.direction;
    Color = mainLight.color;
    DistanceAtten = mainLight.distanceAttenuation;
    ShadowAtten = mainLight.shadowAttenuation;
#endif
}


#ifndef SHADERGRAPH_PREVIEW
// This function gets additional light data and calculates realtime shadows
Light GetAdditionalLightForToon(int pixelLightIndex, float3 worldPosition) {
    // Convert the pixel light index to the light data index
    int perObjectLightIndex = GetPerObjectLightIndex(pixelLightIndex);
    // Call the URP additional light algorithm. This will not calculate shadows, since we don't pass a shadow mask value
    Light light = GetAdditionalPerObjectLight(perObjectLightIndex, worldPosition);
    // Manually set the shadow attenuation by calculating realtime shadows
    light.shadowAttenuation = AdditionalLightRealtimeShadow(perObjectLightIndex, worldPosition);
    return light;
}
#endif

void AddAdditionalLights_float(float Smoothness, float3 WorldPosition, float3 WorldNormal, float3 WorldView,
    float MainDiffuse, float MainSpecular, float3 MainColor, float Steps,
    out float Diffuse, out float Specular, out float3 Color) {
    Diffuse = MainDiffuse;
    Specular = MainSpecular;
    Color = MainColor * (MainDiffuse + MainSpecular);

    float3 addColor = float3(0, 0, 0);

#ifndef SHADERGRAPH_PREVIEW
    int pixelLightCount = GetAdditionalLightsCount();
    for (int i = 0; i < pixelLightCount; ++i) {
        Light light = GetAdditionalLightForToon(i, WorldPosition);
        half NdotL = saturate(dot(WorldNormal, light.direction));
        half atten = light.distanceAttenuation * light.shadowAttenuation;
        half thisDiffuse = atten * NdotL;
        half thisSpecular = LightingSpecular(thisDiffuse, light.direction, WorldNormal, WorldView, 1, Smoothness);
        Diffuse += thisDiffuse;
        Specular += thisSpecular;
        addColor += light.color * (thisDiffuse + thisSpecular);
    }
#endif

    addColor *= Steps;
    addColor = floor(addColor);
    addColor /= Steps;
    Color += addColor;

    half total = Diffuse + Specular;
    // If no light touches this pixel, set the color to the main light's color
    Color = total <= 0 ? MainColor : Color / total;
}

#endif