using Godot;
using MegaCrit.Sts2.Core.Entities.Players;
using STS2RitsuLib.Combat.SecondaryResources;

namespace NineSolsMod.NineSolsModCode.Nodes;

public partial class NQiCounter : Control
{
    private Player? _boundPlayer;
    private int _amount;
    private int? _maxAmount;
    private SecondaryResourceDefinition? _definition;
    private SecondaryResourceIconStyle? _iconStyle;
    private List<NRing> _rings = [];
    private HBoxContainer _hBoxContainer = null!;
    private string _ringTexturePath = $"res://{MainFile.ModId}/images/charui/qi_frame.png";

    public bool AutoRefresh { get; set; }

    public static NQiCounter Create(
        SecondaryResourceDefinition secondaryResourceDefinition,
        SecondaryResourceIconStyle secondaryResourceIconStyle
    )
    {
        var qiCounter = new NQiCounter();
        qiCounter.Configure(secondaryResourceDefinition, secondaryResourceIconStyle);
        return qiCounter;
    }

    public void Configure(SecondaryResourceDefinition secondaryResourceDefinition, SecondaryResourceIconStyle secondaryResourceIconStyle)
    {
        ArgumentNullException.ThrowIfNull(secondaryResourceDefinition);
        _definition = secondaryResourceDefinition;
        _iconStyle = secondaryResourceIconStyle;

        if (IsNodeReady())
        {
            ApplyDefinition();
        }
    }

    public void Bind(Player? player, bool autoRefresh = true)
    {
        if (!ReferenceEquals(_boundPlayer, player))
        {
            Visible = false;
        }

        _boundPlayer = player;
        AutoRefresh = autoRefresh;
        Refresh(_boundPlayer);
    }

    public void Refresh(Player? player)
    {
        if (_definition is null || player is null)
        {
            Visible = false;
            return;
        }

        var amount = SecondaryResourceCmd.Get(player, _definition.Id);
        var maxAmount = SecondaryResourceCmd.GetMax(player, _definition.Id);
        Visible = _definition.IsVisibleInCombatUi(player);
        SetAmount(amount, maxAmount);
    }

    public void SetAmount(int amount, int? maxAmount = null)
    {
        _amount = amount;
        _maxAmount = maxAmount;
        if (!IsNodeReady())
        {
            return;
        }

        if (_rings is null)
        {
            return;
        }

        for (int i = 0; i < _rings.Count; i++)
        {
            if (i < amount)
            {
                if (!_rings[i].IconVisible)
                {
                    _rings[i].ShowIcon();
                }
            }
            else
            {
                if (_rings[i].IconVisible)
                {
                    _rings[i].HideIcon();
                }
            }
        }

        foreach (var ring in _rings)
        {
            ring.SetAmount(_amount, _maxAmount);
        }
    }

    public void ApplyDefinition()
    {
        if (_definition is null || _rings is null)
        {
            return;
        }

        foreach (var ring in _rings)
        {
            ring.Configure(_definition, _iconStyle);
            ring.SetAmount(_amount, _maxAmount);
            ring.Texture = ResourceLoader.Load<Texture2D>(_ringTexturePath);
        }
    }

    public override void _Ready()
    {
        MouseFilter = MouseFilterEnum.Pass;

        _hBoxContainer = new()
        {
            MouseFilter = MouseFilterEnum.Pass
        };
        AddChild(_hBoxContainer);

        for (int i = 0; i < 5; i++)
        {
            var ring = NRing.Create(_definition, _iconStyle);
            _rings.Add(ring);
            _hBoxContainer.AddChild(ring);
        }

        ApplyDefinition();
        SetAmount(_amount, _maxAmount);
    }
}

public partial class NRing : TextureRect
{
    private int _amount;
    private int? _maxAmount;
    private SecondaryResourceDefinition? _definition;
    private SecondaryResourceIconStyle? _iconStyle;
    private NQiIcon? _icon = null;
    private Tween? _tween;
    private float _tweenTime = 0.6f;
    private Vector2 _tweenScale = new(1.8f, 1.8f);
    public bool IconVisible => _icon?.Visible ?? false;

    public bool AutoRefresh { get; set; }

    public static NRing Create(
        SecondaryResourceDefinition secondaryResourceDefinition,
        SecondaryResourceIconStyle secondaryResourceIconStyle
    )
    {
        var ring = new NRing();
        ring.Configure(secondaryResourceDefinition, secondaryResourceIconStyle);
        return ring;
    }

    public void Configure(
        SecondaryResourceDefinition secondaryResourceDefinition,
        SecondaryResourceIconStyle secondaryResourceIconStyle
    )
    {
        ArgumentNullException.ThrowIfNull(secondaryResourceDefinition);
        _definition = secondaryResourceDefinition;
        _iconStyle = secondaryResourceIconStyle;

        if (IsNodeReady())
        {
            ApplyDefinition();
        }
    }

    public void ShowIcon()
    {
        if (_tween is not null && _tween.IsValid())
        {
            _tween.Kill();
        }

        if (_icon is null)
        {
            return;
        }

        _icon.Show();
        _icon.Scale = _tweenScale;
        _icon.Modulate = _icon.Modulate with
        {
            A = 0f
        };

        _tween = CreateTween().SetParallel(true);
        _tween.SetTrans(Tween.TransitionType.Cubic).SetEase(Tween.EaseType.Out);

        _tween.TweenProperty(_icon, "scale", new Vector2(1.0f, 1.0f), _tweenTime);
        _tween.TweenProperty(_icon, "modulate:a", 1.0f, _tweenTime);
    }

    public void HideIcon()
    {
        if (_icon is null)
        {
            return;
        }
        if (!_icon.Visible)
        {
            return;
        }

        if (_tween is not null && _tween.IsValid())
        {
            _tween.Kill();
        }

        _tween = CreateTween().SetParallel(true);
        _tween.SetTrans(Tween.TransitionType.Cubic).SetEase(Tween.EaseType.Out);

        _tween.TweenProperty(_icon, "scale", _tweenScale, _tweenTime);
        _tween.TweenProperty(_icon, "modulate:a", 0.0f, _tweenTime);

        _tween.Chain().TweenCallback(Callable.From(_icon.Hide));
    }


    public void SetAmount(int amount, int? maxAmount = null)
    {
        _amount = amount;
        _maxAmount = maxAmount;
        if (!IsNodeReady())
        {
            return;
        }

        _icon?.SetAmount(_amount, _maxAmount);
    }

    public void ApplyDefinition()
    {
        if (_definition is null || _icon is null)
        {
            return;
        }

        _icon.Configure(_definition, _iconStyle);
        _icon.SetAmount(_amount, _maxAmount);
    }

    public override void _Ready()
    {
        MouseFilter = MouseFilterEnum.Stop;
        Size = _iconStyle?.Size * 1.3f ?? new(56, 56);
        CustomMinimumSize = _iconStyle?.Size * 1.3f ?? new(56, 56);
        ExpandMode = ExpandModeEnum.IgnoreSize;
        StretchMode = StretchModeEnum.KeepAspectCentered;


        _icon = new()
        {
            MouseFilter = MouseFilterEnum.Pass,
        };
        if (_definition is not null)
        {
            _icon.Configure(_definition, SecondaryResourceIconStyle.Default);
        }

        AddChild(_icon);
        ApplyDefinition();
        SetAmount(_amount, _maxAmount);
    }
}

/// <summary>
///     Built-in secondary-resource icon node with consistent texture setup and optional hover-tip wiring.
///     内建次级资源图标节点，统一处理贴图设置和可选悬浮提示绑定。
/// </summary>
public partial class NQiIcon : Control
{
    private int _amount = 1;
    private double _rotationSpeed = 120f;
    private SecondaryResourceDefinition? _definition;
    private SecondaryResourceHoverTipBinder? _hoverTipBinder;
    private int? _maxAmount;
    private SecondaryResourceIconStyle _style = SecondaryResourceIconStyle.Default;
    private TextureRect _texture = null!;

    /// <summary>
    ///     Creates and configures a secondary-resource icon.
    ///     创建并配置次级资源图标。
    /// </summary>
    public static NSecondaryResourceIcon Create(
        SecondaryResourceDefinition definition,
        SecondaryResourceIconStyle? style = null,
        int amount = 1,
        int? maxAmount = null)
    {
        var icon = new NSecondaryResourceIcon();
        icon.Configure(definition, style);
        icon.SetAmount(amount, maxAmount);
        return icon;
    }

    /// <summary>
    ///     Configures the resource definition and visual style.
    ///     配置资源定义和视觉样式。
    /// </summary>
    public void Configure(SecondaryResourceDefinition definition, SecondaryResourceIconStyle? style = null)
    {
        ArgumentNullException.ThrowIfNull(definition);
        _definition = definition;
        _style = style ?? SecondaryResourceIconStyle.Default;
        CustomMinimumSize = _style.Size;
        Size = _style.Size;

        if (!IsNodeReady()) return;
        ApplyStyleAndDefinition();
        RefreshHoverTipBinding();
    }

    /// <summary>
    ///     Updates the amount displayed in this icon's hover tip.
    ///     更新该图标悬浮提示中显示的数量。
    /// </summary>
    public void SetAmount(int amount, int? maxAmount = null)
    {
        _amount = amount;
        _maxAmount = maxAmount;
    }

    /// <inheritdoc />
    public override void _Ready()
    {
        MouseFilter = MouseFilterEnum.Stop;
        _texture = new()
        {
            MouseFilter = MouseFilterEnum.Ignore,
        };
        AddChild(_texture);

        ApplyStyleAndDefinition();
        RefreshHoverTipBinding();
    }

    /// <inheritdoc/>
    public override void _Process(double delta)
    {
        if (_texture is null)
        {
            return;
        }
        _texture.RotationDegrees += (float)(_rotationSpeed * delta);
    }

    /// <inheritdoc />
    public override void _ExitTree()
    {
        _hoverTipBinder?.Hide();
    }

    private void ApplyStyleAndDefinition()
    {
        if (_texture == null)
            return;

        CustomMinimumSize = _style.Size;
        Size = _style.Size;
        Visible = false;
        SetAnchorsAndOffsetsPreset(LayoutPreset.Center, resizeMode: LayoutPresetMode.KeepSize);
        _texture.Position = _style.IconOffset;
        _texture.CustomMinimumSize = _style.Size;
        _texture.Size = _style.Size;
        _texture.ExpandMode = _style.ExpandMode;
        _texture.StretchMode = _style.StretchMode;
        _texture.RotationDegrees = Random.Shared.Next(0, 360);
        _texture.PivotOffset = _texture.Size / 2;

        if (_definition == null)
        {
            _texture.Texture = null;
            return;
        }

        var path = _definition.LargeIconPath ?? _definition.SmallIconPath;
        _texture.Texture = string.IsNullOrWhiteSpace(path) ? null : ResourceLoader.Load<Texture2D>(path);
    }

    private void RefreshHoverTipBinding()
    {
        if (!IsNodeReady())
            return;

        if (_definition == null || _style.HoverTip is not { Enabled: true } hoverTipStyle)
        {
            _hoverTipBinder?.QueueFree();
            _hoverTipBinder = null;
            return;
        }

        if (_hoverTipBinder == null || !IsInstanceValid(_hoverTipBinder))
        {
            _hoverTipBinder = SecondaryResourceHoverTipBinder.Bind(
                this,
                CreateHoverTipRequest,
                hoverTipStyle);
            return;
        }

        _hoverTipBinder.Configure(CreateHoverTipRequest, hoverTipStyle);
    }

    private SecondaryResourceHoverTipRequest? CreateHoverTipRequest()
    {
        return _definition == null ? null : new(_definition, _amount, _maxAmount);
    }
}