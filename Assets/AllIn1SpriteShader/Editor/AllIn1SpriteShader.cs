using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using System.Linq;

[CanEditMultipleObjects]
public class AllIn1SpriteShader : ShaderGUI
{
    private Material targetMat;
    private UnityEngine.Rendering.BlendMode srcMode, dstMode;
    private UnityEngine.Rendering.CompareFunction zTestMode = UnityEngine.Rendering.CompareFunction.LessEqual;

    private GUIStyle style, bigLabel = new GUIStyle();
    private const int bigFontSize = 16;
    private string[] oldKeyWords;
    private int effectCount = 1;

    public override void OnGUI(MaterialEditor materialEditor, MaterialProperty[] properties)
    {
        targetMat = materialEditor.target as Material;
        effectCount = 1;
        oldKeyWords = targetMat.shaderKeywords;
        style = new GUIStyle(EditorStyles.helpBox);
        style.margin = new RectOffset(0, 0, 0, 0);
        bigLabel = new GUIStyle(EditorStyles.boldLabel);
        bigLabel.fontSize = bigFontSize;

        GUILayout.Label("General Properties", bigLabel);
        materialEditor.ShaderProperty(properties[0], properties[0].displayName);
        materialEditor.ShaderProperty(properties[1], properties[1].displayName);
        materialEditor.ShaderProperty(properties[2], properties[2].displayName);

        //Not needed since Unity batches sprites on its own
        //EditorGUILayout.Separator();
        //materialEditor.EnableInstancingField();
        //Debug.Log(materialEditor.IsInstancingEnabled() + "  " + Application.isBatchMode);

        EditorGUILayout.Separator();
        Blending(materialEditor, properties, style, "Custom Blending", "CUSTOMBLENDING_ON");
        Billboard(materialEditor, properties, style, "Bilboard active", "BILBOARD_ON");
        ZWrite(materialEditor, properties, style, "Depth Write");
        ZTest(materialEditor, properties, style, "Z Test", "CUSTOMZTEST_ON");
        GenericEffect(materialEditor, properties, style, "Unity Fog", "FOG_ON", -1, -1, false);
        SpriteAtlas(materialEditor, style, oldKeyWords.Contains("ATLAS_ON"), "Sprite inside an atlas?", "ATLAS_ON");

        DrawLine(Color.grey, 1, 3);
        GUILayout.Label("Color Effects", bigLabel);

        Glow(materialEditor, properties, style, "GLOW_ON");
        GenericEffect(materialEditor, properties, style, "Fade", "FADE_ON", 7, 13);
        Outline(materialEditor, properties, style, "OUTBASE_ON");
        Gradient(materialEditor, properties, style, "Gradient & Radial Gradient", "GRADIENT_ON");
        GenericEffect(materialEditor, properties, style, "Color Swap", "COLORSWAP_ON", 36, 42);
        GenericEffect(materialEditor, properties, style, "Hue Shift", "HSV_ON", 43, 45);
        ColorChange(materialEditor, properties, style, "CHANGECOLOR_ON");
        ColorRamp(materialEditor, properties, style, "COLORRAMP_ON");
        GenericEffect(materialEditor, properties, style, "Hit Effect", "HITEFFECT_ON", 46, 48);
        GenericEffect(materialEditor, properties, style, "Negative", "NEGATIVE_ON", 49, 49);
        GenericEffect(materialEditor, properties, style, "Pixelate", "PIXELATE_ON", 50, 50);
        GreyScale(materialEditor, properties, style, "GREYSCALE_ON");
        Posterize(materialEditor, properties, style, "POSTERIZE_ON");
        Blur(materialEditor, properties, style, "BLUR_ON");
        GenericEffect(materialEditor, properties, style, "Motion Blur", "MOTIONBLUR_ON", 62, 63);
        GenericEffect(materialEditor, properties, style, "Ghost", "GHOST_ON", 64, 65);
        InnerOutline(materialEditor, properties, style, "Inner Outline", "INNEROUTLINE_ON", 66, 69);
        Hologram(materialEditor, properties, style, "Hologram", "HOLOGRAM_ON", 73, 77);
        GenericEffect(materialEditor, properties, style, "Chromatic Aberration", "CHROMABERR_ON", 78, 79);
        Glitch(materialEditor, properties, style, "Glitch", "GLITCH_ON");
        GenericEffect(materialEditor, properties, style, "Flicker", "FLICKER_ON", 81, 83);
        GenericEffect(materialEditor, properties, style, "Shadow", "SHADOW_ON", 84, 87);
        GenericEffect(materialEditor, properties, style, "Shine", "SHINE_ON", 133, 138);
        GenericEffect(materialEditor, properties, style, "Contrast & Brightness", "CONTRAST_ON", 152, 153);
        GenericEffect(materialEditor, properties, style, "Alpha Cutoff", "ALPHACUTOFF_ON", 70, 70);
        GenericEffect(materialEditor, properties, style, "Alpha Round", "ALPHAROUND_ON", 144, 144);

        DrawLine(Color.grey, 1, 3);
        GUILayout.Label("UV Effects", bigLabel);

        GenericEffect(materialEditor, properties, style, "Hand Drawn", "DOODLE_ON", 88, 89);
        Grass(materialEditor, properties, style, "WIND_ON");
        GenericEffect(materialEditor, properties, style, "Wave", "WAVEUV_ON", 94, 98);
        GenericEffect(materialEditor, properties, style, "Round Wave", "ROUNDWAVEUV_ON", 127, 128);
        GenericEffect(materialEditor, properties, style, "Rect Size (Enable wireframe to see result)", "RECTSIZE_ON", 99, 99);
        GenericEffect(materialEditor, properties, style, "Offset", "OFFSETUV_ON", 100, 101);
        GenericEffect(materialEditor, properties, style, "Clipping / Fill Amount", "CLIPPING_ON", 102, 105);
        GenericEffect(materialEditor, properties, style, "Texture Scroll", "TEXTURESCROLL_ON", 106, 107);
        GenericEffect(materialEditor, properties, style, "Zoom", "ZOOMUV_ON", 108, 108);
        GenericEffect(materialEditor, properties, style, "Distortion", "DISTORT_ON", 109, 112);
        GenericEffect(materialEditor, properties, style, "Twist", "TWISTUV_ON", 113, 116);
        GenericEffect(materialEditor, properties, style, "Rotate", "ROTATEUV_ON", 117, 117);
        GenericEffect(materialEditor, properties, style, "Polar Coordinates (Tile texture for good results)", "POLARUV_ON", -1, -1);
        GenericEffect(materialEditor, properties, style, "Fish Eye", "FISHEYE_ON", 118, 118);
        GenericEffect(materialEditor, properties, style, "Pinch", "PINCH_ON", 119, 119);
        GenericEffect(materialEditor, properties, style, "Shake", "SHAKEUV_ON", 120, 122);

        DrawLine(Color.grey, 1, 3);
        materialEditor.RenderQueueField();
    }

    private void Blending(MaterialEditor materialEditor, MaterialProperty[] properties, GUIStyle style, string inspector, string keyword)
    {
        bool toggle = oldKeyWords.Contains(keyword);
        MaterialProperty srcM = ShaderGUI.FindProperty("_MySrcMode", properties);
        MaterialProperty dstM = ShaderGUI.FindProperty("_MyDstMode", properties);
        if (srcM.floatValue == 0 && dstM.floatValue == 0)
        {
            srcM.floatValue = 5;
            dstM.floatValue = 10;
        }
        bool ini = toggle;
        toggle = EditorGUILayout.BeginToggleGroup(inspector, toggle);
        if (ini != toggle && !Application.isPlaying) EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        if (toggle)
        {
            targetMat.EnableKeyword(keyword);
            EditorGUILayout.BeginVertical(style);
            {
                GUILayout.Label("Look for 'ShaderLab: Blending' if you don't know what this is", style);
                if (GUILayout.Button("Back To Default Blending"))
                {
                    srcM.floatValue = 5;
                    dstM.floatValue = 10;
                    targetMat.DisableKeyword("PREMULTIPLYALPHA_ON");
                }
                srcMode = (UnityEngine.Rendering.BlendMode)srcM.floatValue;
                dstMode = (UnityEngine.Rendering.BlendMode)dstM.floatValue;
                srcMode = (UnityEngine.Rendering.BlendMode)EditorGUILayout.EnumPopup("SrcMode", srcMode);
                dstMode = (UnityEngine.Rendering.BlendMode)EditorGUILayout.EnumPopup("DstMode", dstMode);
                srcM.floatValue = (float)(srcMode);
                dstM.floatValue = (float)(dstMode);

                ini = oldKeyWords.Contains("PREMULTIPLYALPHA_ON");
                toggle = EditorGUILayout.Toggle("Premultiply Alpha?", ini);
                if (ini != toggle && !Application.isPlaying) EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
                if (toggle) targetMat.EnableKeyword("PREMULTIPLYALPHA_ON");
                else targetMat.DisableKeyword("PREMULTIPLYALPHA_ON");
            }
            EditorGUILayout.EndVertical();
        }
        else
        {
            srcM.floatValue = 5;
            dstM.floatValue = 10;
            targetMat.DisableKeyword(keyword);
        }
        EditorGUILayout.EndToggleGroup();
    }

    private void Billboard(MaterialEditor materialEditor, MaterialProperty[] properties, GUIStyle style, string inspector, string keyword)
    {
        bool toggle = oldKeyWords.Contains(keyword);
        bool ini = toggle;
        toggle = EditorGUILayout.BeginToggleGroup(inspector, toggle);
        if (ini != toggle && !Application.isPlaying) EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        if (toggle)
        {
            targetMat.EnableKeyword(keyword);
            EditorGUILayout.BeginVertical(style);
            {
                GUILayout.Label("Don't use this feature on UI elements!", style);
                materialEditor.ShaderProperty(properties[129], properties[129].displayName);
                MaterialProperty billboardY = properties[129];
                if (billboardY.floatValue == 1) targetMat.EnableKeyword("BILBOARDY_ON");
                else targetMat.DisableKeyword("BILBOARDY_ON");
            }
            EditorGUILayout.EndVertical();
        }
        else targetMat.DisableKeyword(keyword);
        EditorGUILayout.EndToggleGroup();
    }

    private void ZWrite(MaterialEditor materialEditor, MaterialProperty[] properties, GUIStyle style, string inspector)
    {
        MaterialProperty zWrite = ShaderGUI.FindProperty("_ZWrite", properties);
        bool toggle = zWrite.floatValue > 0.9f ? true : false;
        bool ini = toggle;
        toggle = EditorGUILayout.BeginToggleGroup(inspector, toggle);
        if (ini != toggle && !Application.isPlaying) EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        if (toggle)
        {
            EditorGUILayout.BeginVertical(style);
            {
                GUILayout.Label("Usually used to sort Billboarded sprites", style);
                GUILayout.Label("Use with Alpha Cutoff effect for more optimum results", style);
                zWrite.floatValue = 1.0f;
            }
            EditorGUILayout.EndVertical();
        }
        else zWrite.floatValue = 0.0f;
        EditorGUILayout.EndToggleGroup();
    }

    private void ZTest(MaterialEditor materialEditor, MaterialProperty[] properties, GUIStyle style, string inspector, string keyword)
    {
        bool toggle = oldKeyWords.Contains(keyword);
        MaterialProperty zTestM = ShaderGUI.FindProperty("_ZTestMode", properties);
        if (zTestM.floatValue == 0) zTestM.floatValue = 4;
        bool ini = toggle;
        toggle = EditorGUILayout.BeginToggleGroup(inspector, toggle);
        if (ini != toggle && !Application.isPlaying) EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        if (toggle)
        {
            targetMat.EnableKeyword(keyword);
            EditorGUILayout.BeginVertical(style);
            {
                GUILayout.Label("Look for 'ShaderLab culling and depth testing' if you don't know what this is", style);
                zTestMode = (UnityEngine.Rendering.CompareFunction)zTestM.floatValue;
                zTestMode = (UnityEngine.Rendering.CompareFunction)EditorGUILayout.EnumPopup("zTestMode", zTestMode);
                zTestM.floatValue = (float)(zTestMode);
            }
            EditorGUILayout.EndVertical();
        }
        else targetMat.DisableKeyword(keyword);
        EditorGUILayout.EndToggleGroup();
    }

    private void SpriteAtlas(MaterialEditor materialEditor, GUIStyle style, bool toggle, string inspector, string keyword)
    {
        bool ini = toggle;
        toggle = EditorGUILayout.BeginToggleGroup(inspector, toggle);
        if (ini != toggle && !Application.isPlaying) EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        if (toggle)
        {
            targetMat.EnableKeyword(keyword);
            EditorGUILayout.BeginVertical(style);
            {
                GUILayout.Label("Make sure SpriteAtlasUV component is added \n " +
                    "*Check documentation if unsure what this does or how it works", style);
            }
            EditorGUILayout.EndVertical();
        }
        else targetMat.DisableKeyword(keyword);
        EditorGUILayout.EndToggleGroup();
    }

    private void Outline(MaterialEditor materialEditor, MaterialProperty[] properties, GUIStyle style, string keyword)
    {
        bool toggle = oldKeyWords.Contains(keyword);
        bool ini = toggle;
        toggle = EditorGUILayout.BeginToggleGroup(effectCount + ".Outline", toggle);
        effectCount++;
        if (ini != toggle && !Application.isPlaying) EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        if (toggle)
        {
            targetMat.EnableKeyword("OUTBASE_ON");
            EditorGUILayout.BeginVertical(style);
            {
                materialEditor.ShaderProperty(properties[14], properties[14].displayName);
                materialEditor.ShaderProperty(properties[15], properties[15].displayName);
                materialEditor.ShaderProperty(properties[16], properties[16].displayName);
                materialEditor.ShaderProperty(properties[17], properties[17].displayName);
                MaterialProperty outline8dir = properties[17];
                if (outline8dir.floatValue == 1) targetMat.EnableKeyword("OUTBASE8DIR_ON");
                else targetMat.DisableKeyword("OUTBASE8DIR_ON");

                materialEditor.ShaderProperty(properties[19], properties[19].displayName);
                MaterialProperty outlinePixel = properties[19];
                if (outlinePixel.floatValue == 1)
                {
                    targetMat.EnableKeyword("OUTBASEPIXELPERF_ON");
                    materialEditor.ShaderProperty(properties[20], properties[20].displayName);
                }
                else
                {
                    targetMat.DisableKeyword("OUTBASEPIXELPERF_ON");
                    materialEditor.ShaderProperty(properties[18], properties[18].displayName);
                }

                materialEditor.ShaderProperty(properties[21], properties[21].displayName);
                MaterialProperty outlineTex = properties[21];
                if (outlineTex.floatValue == 1)
                {
                    targetMat.EnableKeyword("OUTTEX_ON");
                    materialEditor.ShaderProperty(properties[22], properties[22].displayName);
                    materialEditor.ShaderProperty(properties[23], properties[23].displayName);
                    materialEditor.ShaderProperty(properties[24], properties[24].displayName);
                    materialEditor.ShaderProperty(properties[25], properties[25].displayName);
                    MaterialProperty outlineTexGrey = properties[25];
                    if (outlineTexGrey.floatValue == 1) targetMat.EnableKeyword("OUTGREYTEXTURE_ON");
                    else targetMat.DisableKeyword("OUTGREYTEXTURE_ON");
                }
                else targetMat.DisableKeyword("OUTTEX_ON");

                materialEditor.ShaderProperty(properties[26], properties[26].displayName);
                MaterialProperty outlineDistort = properties[26];
                if (outlineDistort.floatValue == 1)
                {
                    targetMat.EnableKeyword("OUTDIST_ON");
                    materialEditor.ShaderProperty(properties[27], properties[27].displayName);
                    materialEditor.ShaderProperty(properties[28], properties[28].displayName);
                    materialEditor.ShaderProperty(properties[29], properties[29].displayName);
                    materialEditor.ShaderProperty(properties[30], properties[30].displayName);
                }
                else targetMat.DisableKeyword("OUTDIST_ON");

                EditorGUILayout.Separator();
                materialEditor.ShaderProperty(properties[71], properties[71].displayName);
                MaterialProperty onlyOutline = properties[71];
                if (onlyOutline.floatValue == 1) targetMat.EnableKeyword("ONLYOUTLINE_ON");
                else targetMat.DisableKeyword("ONLYOUTLINE_ON");
            }
            EditorGUILayout.EndVertical();
        }
        else targetMat.DisableKeyword("OUTBASE_ON");
        EditorGUILayout.EndToggleGroup();
    }

    private void GenericEffect(MaterialEditor materialEditor, MaterialProperty[] properties, GUIStyle style, string inspector, string keyword, int first, int last, bool effectCounter = true)
    {
        bool toggle = oldKeyWords.Contains(keyword);
        bool ini = toggle;
        if (effectCounter)
        {
            toggle = EditorGUILayout.BeginToggleGroup(effectCount + "." + inspector, toggle);
            effectCount++;
        }
        else toggle = EditorGUILayout.BeginToggleGroup(inspector, toggle);
        if (ini != toggle && !Application.isPlaying) EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        if (toggle)
        {
            targetMat.EnableKeyword(keyword);
            if (first > 0)
            {
                EditorGUILayout.BeginVertical(style);
                {
                    for (int i = first; i <= last; i++) materialEditor.ShaderProperty(properties[i], properties[i].displayName);
                }
                EditorGUILayout.EndVertical();
            }
        }
        else targetMat.DisableKeyword(keyword);
        EditorGUILayout.EndToggleGroup();
    }

    private void Glow(MaterialEditor materialEditor, MaterialProperty[] properties, GUIStyle style, string keyword)
    {
        bool toggle = oldKeyWords.Contains(keyword);
        bool ini = toggle;
        toggle = EditorGUILayout.BeginToggleGroup(effectCount + ".Glow", toggle);
        effectCount++;
        if (ini != toggle && !Application.isPlaying) EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        if (toggle)
        {
            targetMat.EnableKeyword("GLOW_ON");
            EditorGUILayout.BeginVertical(style);
            {
                materialEditor.ShaderProperty(properties[3], properties[3].displayName);
                materialEditor.ShaderProperty(properties[4], properties[4].displayName);
                materialEditor.ShaderProperty(properties[5], properties[5].displayName);
                MaterialProperty useGlowTex = properties[5];
                if (useGlowTex.floatValue == 1)
                {
                    targetMat.EnableKeyword("GLOWTEX_ON");
                    materialEditor.ShaderProperty(properties[6], properties[6].displayName);
                }
                else targetMat.DisableKeyword("GLOWTEX_ON");
            }
            EditorGUILayout.EndVertical();
        }
        else targetMat.DisableKeyword("GLOW_ON");
        EditorGUILayout.EndToggleGroup();
    }

    private void ColorRamp(MaterialEditor materialEditor, MaterialProperty[] properties, GUIStyle style, string keyword)
    {
        bool toggle = oldKeyWords.Contains(keyword);
        bool ini = toggle;
        toggle = EditorGUILayout.BeginToggleGroup(effectCount + ".Color Ramp", toggle);
        effectCount++;
        if (ini != toggle && !Application.isPlaying) EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        if (toggle)
        {
            targetMat.EnableKeyword("COLORRAMP_ON");
            EditorGUILayout.BeginVertical(style);
            {
                materialEditor.ShaderProperty(properties[51], properties[51].displayName);
                materialEditor.ShaderProperty(properties[52], properties[52].displayName);
                materialEditor.ShaderProperty(properties[53], properties[53].displayName);
                MaterialProperty colorRampOut = properties[53];
                if (colorRampOut.floatValue == 1) targetMat.EnableKeyword("COLORRAMPOUTLINE_ON");
                else targetMat.DisableKeyword("COLORRAMPOUTLINE_ON");
            }
            EditorGUILayout.EndVertical();
        }
        else targetMat.DisableKeyword("COLORRAMP_ON");
        EditorGUILayout.EndToggleGroup();
    }

    private void ColorChange(MaterialEditor materialEditor, MaterialProperty[] properties, GUIStyle style, string keyword)
    {
        bool toggle = oldKeyWords.Contains(keyword);
        bool ini = toggle;
        toggle = EditorGUILayout.BeginToggleGroup(effectCount + ".Change 1 Color", toggle);
        effectCount++;
        if (ini != toggle && !Application.isPlaying) EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        if (toggle)
        {
            targetMat.EnableKeyword("CHANGECOLOR_ON");
            EditorGUILayout.BeginVertical(style);
            {
                for (int i = 123; i < 127; i++) materialEditor.ShaderProperty(properties[i], properties[i].displayName);

                EditorGUILayout.Separator();
                ini = oldKeyWords.Contains("CHANGECOLOR2_ON");
                bool toggle2 = ini;
                toggle2 = EditorGUILayout.Toggle("Use Color 2", ini);
                if (ini != toggle2 && !Application.isPlaying) EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
                if (toggle2)
                {
                    targetMat.EnableKeyword("CHANGECOLOR2_ON");
                    for (int i = 146; i < 149; i++) materialEditor.ShaderProperty(properties[i], properties[i].displayName);
                }
                else targetMat.DisableKeyword("CHANGECOLOR2_ON");

                EditorGUILayout.Separator();
                ini = oldKeyWords.Contains("CHANGECOLOR3_ON");
                toggle2 = ini;
                toggle2 = EditorGUILayout.Toggle("Use Color 3", toggle2);
                if (ini != toggle2 && !Application.isPlaying) EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
                if (toggle2)
                {
                    targetMat.EnableKeyword("CHANGECOLOR3_ON");
                    for (int i = 149; i < 152; i++) materialEditor.ShaderProperty(properties[i], properties[i].displayName);
                }
                else targetMat.DisableKeyword("CHANGECOLOR3_ON");
            }
            EditorGUILayout.EndVertical();
        }
        else targetMat.DisableKeyword("CHANGECOLOR_ON");
        EditorGUILayout.EndToggleGroup();
    }

    private void GreyScale(MaterialEditor materialEditor, MaterialProperty[] properties, GUIStyle style, string keyword)
    {
        bool toggle = oldKeyWords.Contains(keyword);
        bool ini = toggle;
        toggle = EditorGUILayout.BeginToggleGroup(effectCount + ".Greyscale", toggle);
        effectCount++;
        if (ini != toggle && !Application.isPlaying) EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        if (toggle)
        {
            targetMat.EnableKeyword("GREYSCALE_ON");
            EditorGUILayout.BeginVertical(style);
            {
                materialEditor.ShaderProperty(properties[54], properties[54].displayName);
                materialEditor.ShaderProperty(properties[56], properties[56].displayName);
                materialEditor.ShaderProperty(properties[55], properties[55].displayName);
                MaterialProperty greyScaleOut = properties[55];
                if (greyScaleOut.floatValue == 1) targetMat.EnableKeyword("GREYSCALEOUTLINE_ON");
                else targetMat.DisableKeyword("GREYSCALEOUTLINE_ON");
            }
            EditorGUILayout.EndVertical();
        }
        else targetMat.DisableKeyword("GREYSCALE_ON");
        EditorGUILayout.EndToggleGroup();
    }

    private void Posterize(MaterialEditor materialEditor, MaterialProperty[] properties, GUIStyle style, string keyword)
    {
        bool toggle = oldKeyWords.Contains(keyword);
        bool ini = toggle;
        toggle = EditorGUILayout.BeginToggleGroup(effectCount + ".Posterize", toggle);
        effectCount++;
        if (ini != toggle && !Application.isPlaying) EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        if (toggle)
        {
            targetMat.EnableKeyword("POSTERIZE_ON");
            EditorGUILayout.BeginVertical(style);
            {
                materialEditor.ShaderProperty(properties[57], properties[57].displayName);
                materialEditor.ShaderProperty(properties[58], properties[58].displayName);
                materialEditor.ShaderProperty(properties[59], properties[59].displayName);
                MaterialProperty posterizeOut = properties[59];
                if (posterizeOut.floatValue == 1) targetMat.EnableKeyword("POSTERIZEOUTLINE_ON");
                else targetMat.DisableKeyword("POSTERIZEOUTLINE_ON");
            }
            EditorGUILayout.EndVertical();
        }
        else targetMat.DisableKeyword("POSTERIZE_ON");
        EditorGUILayout.EndToggleGroup();
    }

    private void Blur(MaterialEditor materialEditor, MaterialProperty[] properties, GUIStyle style, string keyword)
    {
        bool toggle = oldKeyWords.Contains(keyword);
        bool ini = toggle;
        toggle = EditorGUILayout.BeginToggleGroup(effectCount + ".Blur", toggle);
        effectCount++;
        if (ini != toggle && !Application.isPlaying) EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        if (toggle)
        {
            targetMat.EnableKeyword("BLUR_ON");
            EditorGUILayout.BeginVertical(style);
            {
                materialEditor.ShaderProperty(properties[60], properties[60].displayName);
                materialEditor.ShaderProperty(properties[61], properties[61].displayName);
                MaterialProperty blurIsHd = properties[61];
                if (blurIsHd.floatValue == 1) targetMat.EnableKeyword("BLURISHD_ON");
                else targetMat.DisableKeyword("BLURISHD_ON");
            }
            EditorGUILayout.EndVertical();
        }
        else targetMat.DisableKeyword("BLUR_ON");
        EditorGUILayout.EndToggleGroup();
    }

    private void Grass(MaterialEditor materialEditor, MaterialProperty[] properties, GUIStyle style, string keyword)
    {
        bool toggle = oldKeyWords.Contains(keyword);
        bool ini = toggle;
        toggle = EditorGUILayout.BeginToggleGroup(effectCount + ".Grass Movement / Wind", toggle);
        effectCount++;
        if (ini != toggle && !Application.isPlaying) EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        if (toggle)
        {
            targetMat.EnableKeyword("WIND_ON");
            EditorGUILayout.BeginVertical(style);
            {
                materialEditor.ShaderProperty(properties[90], properties[90].displayName);
                materialEditor.ShaderProperty(properties[91], properties[91].displayName);
                materialEditor.ShaderProperty(properties[145], properties[145].displayName);
                materialEditor.ShaderProperty(properties[92], properties[92].displayName);
                materialEditor.ShaderProperty(properties[93], properties[93].displayName);
                MaterialProperty grassManual = properties[92];
                if (grassManual.floatValue == 1) targetMat.EnableKeyword("MANUALWIND_ON");
                else targetMat.DisableKeyword("MANUALWIND_ON");
            }
            EditorGUILayout.EndVertical();
        }
        else targetMat.DisableKeyword("WIND_ON");
        EditorGUILayout.EndToggleGroup();
    }

    private void InnerOutline(MaterialEditor materialEditor, MaterialProperty[] properties, GUIStyle style, string inspector, string keyword, int first, int last)
    {
        bool toggle = oldKeyWords.Contains(keyword);
        bool ini = toggle;
        toggle = EditorGUILayout.BeginToggleGroup(effectCount + "." + inspector, toggle);
        effectCount++;
        if (ini != toggle && !Application.isPlaying) EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        if (toggle)
        {
            targetMat.EnableKeyword(keyword);
            if (first > 0)
            {
                EditorGUILayout.BeginVertical(style);
                {
                    for (int i = first; i <= last; i++) materialEditor.ShaderProperty(properties[i], properties[i].displayName);

                    EditorGUILayout.Separator();
                    materialEditor.ShaderProperty(properties[72], properties[72].displayName);
                    MaterialProperty onlyInOutline = properties[72];
                    if (onlyInOutline.floatValue == 1) targetMat.EnableKeyword("ONLYINNEROUTLINE_ON");
                    else targetMat.DisableKeyword("ONLYINNEROUTLINE_ON");
                }
                EditorGUILayout.EndVertical();
            }
        }
        else targetMat.DisableKeyword(keyword);
        EditorGUILayout.EndToggleGroup();
    }

    private void Glitch(MaterialEditor materialEditor, MaterialProperty[] properties, GUIStyle style, string inspector, string keyword)
    {
        bool toggle = oldKeyWords.Contains(keyword);
        bool ini = toggle;
        toggle = EditorGUILayout.BeginToggleGroup(effectCount + "." + inspector, toggle);
        effectCount++;
        if (ini != toggle && !Application.isPlaying) EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        if (toggle)
        {
            targetMat.EnableKeyword(keyword);
            EditorGUILayout.BeginVertical(style);
            {
                materialEditor.ShaderProperty(properties[80], properties[80].displayName);
                materialEditor.ShaderProperty(properties[139], properties[139].displayName);
            }
            EditorGUILayout.EndVertical();
        }
        else targetMat.DisableKeyword(keyword);
        EditorGUILayout.EndToggleGroup();
    }

    private void Hologram(MaterialEditor materialEditor, MaterialProperty[] properties, GUIStyle style, string inspector, string keyword, int first, int last)
    {
        bool toggle = oldKeyWords.Contains(keyword);
        bool ini = toggle;
        toggle = EditorGUILayout.BeginToggleGroup(effectCount + "." + inspector, toggle);
        effectCount++;
        if (ini != toggle && !Application.isPlaying) EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        if (toggle)
        {
            targetMat.EnableKeyword(keyword);
            if (first > 0)
            {
                EditorGUILayout.BeginVertical(style);
                {
                    for (int i = first; i <= last; i++) materialEditor.ShaderProperty(properties[i], properties[i].displayName);
                    materialEditor.ShaderProperty(properties[140], properties[140].displayName);
                }
                EditorGUILayout.EndVertical();
            }
        }
        else targetMat.DisableKeyword(keyword);
        EditorGUILayout.EndToggleGroup();
    }

    private void Gradient(MaterialEditor materialEditor, MaterialProperty[] properties, GUIStyle style, string inspector, string keyword)
    {
        bool toggle = oldKeyWords.Contains(keyword);
        bool ini = toggle;
        toggle = EditorGUILayout.BeginToggleGroup(effectCount + "." + inspector, toggle);
        effectCount++;
        if (ini != toggle && !Application.isPlaying) EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        if (toggle)
        {
            targetMat.EnableKeyword(keyword);

            EditorGUILayout.BeginVertical(style);
            {
                materialEditor.ShaderProperty(properties[143], properties[143].displayName);
                MaterialProperty gradIsRadial = properties[143];
                if (gradIsRadial.floatValue == 1)
                {
                    targetMat.EnableKeyword("RADIALGRADIENT_ON");
                    materialEditor.ShaderProperty(properties[31], properties[31].displayName);
                    materialEditor.ShaderProperty(properties[32], properties[32].displayName);
                    materialEditor.ShaderProperty(properties[34], properties[34].displayName);
                    materialEditor.ShaderProperty(properties[141], properties[141].displayName);
                }
                else
                {
                    targetMat.DisableKeyword("RADIALGRADIENT_ON");
                    bool simpleGradient = oldKeyWords.Contains("GRADIENT2COL_ON");
                    bool simpleGradToggle = EditorGUILayout.Toggle("2 Color Gradient?", simpleGradient);
                    if(simpleGradient && !simpleGradToggle) targetMat.DisableKeyword("GRADIENT2COL_ON");
                    else if(!simpleGradient && simpleGradToggle) targetMat.EnableKeyword("GRADIENT2COL_ON");
                    materialEditor.ShaderProperty(properties[31], properties[31].displayName);
                    materialEditor.ShaderProperty(properties[32], properties[32].displayName);
                    if(!simpleGradToggle) materialEditor.ShaderProperty(properties[33], properties[33].displayName);
                    materialEditor.ShaderProperty(properties[34], properties[34].displayName);
                    if (!simpleGradToggle) materialEditor.ShaderProperty(properties[35], properties[35].displayName);
                    if (!simpleGradToggle) materialEditor.ShaderProperty(properties[141], properties[141].displayName);
                    materialEditor.ShaderProperty(properties[142], properties[142].displayName);
                }
            }
            EditorGUILayout.EndVertical();
        }
        else targetMat.DisableKeyword(keyword);
        EditorGUILayout.EndToggleGroup();
    }

    private void DrawLine(Color color, int thickness = 2, int padding = 10)
    {
        Rect r = EditorGUILayout.GetControlRect(GUILayout.Height(padding + thickness));
        r.height = thickness;
        r.y += (padding / 2);
        r.x -= 2;
        r.width += 6;
        EditorGUI.DrawRect(r, color);
    }
}