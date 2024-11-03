// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: playerInfo.proto
// </auto-generated>
#pragma warning disable 1591, 0612, 3021, 8981
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using pbr = global::Google.Protobuf.Reflection;
using scg = global::System.Collections.Generic;
namespace PlayerInfo {

  /// <summary>Holder for reflection information generated from playerInfo.proto</summary>
  public static partial class PlayerInfoReflection {

    #region Descriptor
    /// <summary>File descriptor for playerInfo.proto</summary>
    public static pbr::FileDescriptor Descriptor {
      get { return descriptor; }
    }
    private static pbr::FileDescriptor descriptor;

    static PlayerInfoReflection() {
      byte[] descriptorData = global::System.Convert.FromBase64String(
          string.Concat(
            "ChBwbGF5ZXJJbmZvLnByb3RvEgpwbGF5ZXJJbmZvIoIBCgpQbGF5ZXJJbmZv",
            "EhAKCHBsYXllcklkGAEgASgJEgkKAXgYAiABKAISCQoBeRgDIAEoAhIJCgF6",
            "GAQgASgCEgoKAnJ4GAUgASgCEgoKAnJ5GAYgASgCEgoKAnJ6GAcgASgCEg0K",
            "BXNwZWVkGAggASgCEg4KBmhlYWx0aBgJIAEoAio4CgpQcmVmYWJUeXBlEhIK",
            "DlRZUEVfVU5ERUZJTkVEEAASCgoGVFlQRV9BEAESCgoGVFlQRV9CEAJCEloQ",
            "c2VydmVyLWdvL3NyYy9wYmIGcHJvdG8z"));
      descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
          new pbr::FileDescriptor[] { },
          new pbr::GeneratedClrTypeInfo(new[] {typeof(global::PlayerInfo.PrefabType), }, null, new pbr::GeneratedClrTypeInfo[] {
            new pbr::GeneratedClrTypeInfo(typeof(global::PlayerInfo.PlayerInfo), global::PlayerInfo.PlayerInfo.Parser, new[]{ "PlayerId", "X", "Y", "Z", "Rx", "Ry", "Rz", "Speed", "Health" }, null, null, null, null)
          }));
    }
    #endregion

  }
  #region Enums
  /// <summary>
  /// 프리팹 타입 정의 (딱 2가지 타입)
  /// </summary>
  public enum PrefabType {
    /// <summary>
    /// 기본값 (정의되지 않은 경우)
    /// </summary>
    [pbr::OriginalName("TYPE_UNDEFINED")] TypeUndefined = 0,
    /// <summary>
    /// 첫 번째 프리팹 타입
    /// </summary>
    [pbr::OriginalName("TYPE_A")] TypeA = 1,
    /// <summary>
    /// 두 번째 프리팹 타입
    /// </summary>
    [pbr::OriginalName("TYPE_B")] TypeB = 2,
  }

  #endregion

  #region Messages
  /// <summary>
  /// 요청, 응답으로 사용할 플레이어의 정보
  /// </summary>
  [global::System.Diagnostics.DebuggerDisplayAttribute("{ToString(),nq}")]
  public sealed partial class PlayerInfo : pb::IMessage<PlayerInfo>
  #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      , pb::IBufferMessage
  #endif
  {
    private static readonly pb::MessageParser<PlayerInfo> _parser = new pb::MessageParser<PlayerInfo>(() => new PlayerInfo());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public static pb::MessageParser<PlayerInfo> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::PlayerInfo.PlayerInfoReflection.Descriptor.MessageTypes[0]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public PlayerInfo() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public PlayerInfo(PlayerInfo other) : this() {
      playerId_ = other.playerId_;
      x_ = other.x_;
      y_ = other.y_;
      z_ = other.z_;
      rx_ = other.rx_;
      ry_ = other.ry_;
      rz_ = other.rz_;
      speed_ = other.speed_;
      health_ = other.health_;
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public PlayerInfo Clone() {
      return new PlayerInfo(this);
    }

    /// <summary>Field number for the "playerId" field.</summary>
    public const int PlayerIdFieldNumber = 1;
    private string playerId_ = "";
    /// <summary>
    /// 플레이어의 고유 ID
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public string PlayerId {
      get { return playerId_; }
      set {
        playerId_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "x" field.</summary>
    public const int XFieldNumber = 2;
    private float x_;
    /// <summary>
    /// X 좌표
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public float X {
      get { return x_; }
      set {
        x_ = value;
      }
    }

    /// <summary>Field number for the "y" field.</summary>
    public const int YFieldNumber = 3;
    private float y_;
    /// <summary>
    /// Y 좌표
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public float Y {
      get { return y_; }
      set {
        y_ = value;
      }
    }

    /// <summary>Field number for the "z" field.</summary>
    public const int ZFieldNumber = 4;
    private float z_;
    /// <summary>
    /// Z 좌표
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public float Z {
      get { return z_; }
      set {
        z_ = value;
      }
    }

    /// <summary>Field number for the "rx" field.</summary>
    public const int RxFieldNumber = 5;
    private float rx_;
    /// <summary>
    /// X 축 회전
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public float Rx {
      get { return rx_; }
      set {
        rx_ = value;
      }
    }

    /// <summary>Field number for the "ry" field.</summary>
    public const int RyFieldNumber = 6;
    private float ry_;
    /// <summary>
    /// Y 축 회전
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public float Ry {
      get { return ry_; }
      set {
        ry_ = value;
      }
    }

    /// <summary>Field number for the "rz" field.</summary>
    public const int RzFieldNumber = 7;
    private float rz_;
    /// <summary>
    /// Z 축 회전
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public float Rz {
      get { return rz_; }
      set {
        rz_ = value;
      }
    }

    /// <summary>Field number for the "speed" field.</summary>
    public const int SpeedFieldNumber = 8;
    private float speed_;
    /// <summary>
    /// 플레이어 속도
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public float Speed {
      get { return speed_; }
      set {
        speed_ = value;
      }
    }

    /// <summary>Field number for the "health" field.</summary>
    public const int HealthFieldNumber = 9;
    private float health_;
    /// <summary>
    /// 플레이어 체력
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public float Health {
      get { return health_; }
      set {
        health_ = value;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override bool Equals(object other) {
      return Equals(other as PlayerInfo);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public bool Equals(PlayerInfo other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (PlayerId != other.PlayerId) return false;
      if (!pbc::ProtobufEqualityComparers.BitwiseSingleEqualityComparer.Equals(X, other.X)) return false;
      if (!pbc::ProtobufEqualityComparers.BitwiseSingleEqualityComparer.Equals(Y, other.Y)) return false;
      if (!pbc::ProtobufEqualityComparers.BitwiseSingleEqualityComparer.Equals(Z, other.Z)) return false;
      if (!pbc::ProtobufEqualityComparers.BitwiseSingleEqualityComparer.Equals(Rx, other.Rx)) return false;
      if (!pbc::ProtobufEqualityComparers.BitwiseSingleEqualityComparer.Equals(Ry, other.Ry)) return false;
      if (!pbc::ProtobufEqualityComparers.BitwiseSingleEqualityComparer.Equals(Rz, other.Rz)) return false;
      if (!pbc::ProtobufEqualityComparers.BitwiseSingleEqualityComparer.Equals(Speed, other.Speed)) return false;
      if (!pbc::ProtobufEqualityComparers.BitwiseSingleEqualityComparer.Equals(Health, other.Health)) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override int GetHashCode() {
      int hash = 1;
      if (PlayerId.Length != 0) hash ^= PlayerId.GetHashCode();
      if (X != 0F) hash ^= pbc::ProtobufEqualityComparers.BitwiseSingleEqualityComparer.GetHashCode(X);
      if (Y != 0F) hash ^= pbc::ProtobufEqualityComparers.BitwiseSingleEqualityComparer.GetHashCode(Y);
      if (Z != 0F) hash ^= pbc::ProtobufEqualityComparers.BitwiseSingleEqualityComparer.GetHashCode(Z);
      if (Rx != 0F) hash ^= pbc::ProtobufEqualityComparers.BitwiseSingleEqualityComparer.GetHashCode(Rx);
      if (Ry != 0F) hash ^= pbc::ProtobufEqualityComparers.BitwiseSingleEqualityComparer.GetHashCode(Ry);
      if (Rz != 0F) hash ^= pbc::ProtobufEqualityComparers.BitwiseSingleEqualityComparer.GetHashCode(Rz);
      if (Speed != 0F) hash ^= pbc::ProtobufEqualityComparers.BitwiseSingleEqualityComparer.GetHashCode(Speed);
      if (Health != 0F) hash ^= pbc::ProtobufEqualityComparers.BitwiseSingleEqualityComparer.GetHashCode(Health);
      if (_unknownFields != null) {
        hash ^= _unknownFields.GetHashCode();
      }
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public void WriteTo(pb::CodedOutputStream output) {
    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      output.WriteRawMessage(this);
    #else
      if (PlayerId.Length != 0) {
        output.WriteRawTag(10);
        output.WriteString(PlayerId);
      }
      if (X != 0F) {
        output.WriteRawTag(21);
        output.WriteFloat(X);
      }
      if (Y != 0F) {
        output.WriteRawTag(29);
        output.WriteFloat(Y);
      }
      if (Z != 0F) {
        output.WriteRawTag(37);
        output.WriteFloat(Z);
      }
      if (Rx != 0F) {
        output.WriteRawTag(45);
        output.WriteFloat(Rx);
      }
      if (Ry != 0F) {
        output.WriteRawTag(53);
        output.WriteFloat(Ry);
      }
      if (Rz != 0F) {
        output.WriteRawTag(61);
        output.WriteFloat(Rz);
      }
      if (Speed != 0F) {
        output.WriteRawTag(69);
        output.WriteFloat(Speed);
      }
      if (Health != 0F) {
        output.WriteRawTag(77);
        output.WriteFloat(Health);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    #endif
    }

    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    void pb::IBufferMessage.InternalWriteTo(ref pb::WriteContext output) {
      if (PlayerId.Length != 0) {
        output.WriteRawTag(10);
        output.WriteString(PlayerId);
      }
      if (X != 0F) {
        output.WriteRawTag(21);
        output.WriteFloat(X);
      }
      if (Y != 0F) {
        output.WriteRawTag(29);
        output.WriteFloat(Y);
      }
      if (Z != 0F) {
        output.WriteRawTag(37);
        output.WriteFloat(Z);
      }
      if (Rx != 0F) {
        output.WriteRawTag(45);
        output.WriteFloat(Rx);
      }
      if (Ry != 0F) {
        output.WriteRawTag(53);
        output.WriteFloat(Ry);
      }
      if (Rz != 0F) {
        output.WriteRawTag(61);
        output.WriteFloat(Rz);
      }
      if (Speed != 0F) {
        output.WriteRawTag(69);
        output.WriteFloat(Speed);
      }
      if (Health != 0F) {
        output.WriteRawTag(77);
        output.WriteFloat(Health);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(ref output);
      }
    }
    #endif

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public int CalculateSize() {
      int size = 0;
      if (PlayerId.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(PlayerId);
      }
      if (X != 0F) {
        size += 1 + 4;
      }
      if (Y != 0F) {
        size += 1 + 4;
      }
      if (Z != 0F) {
        size += 1 + 4;
      }
      if (Rx != 0F) {
        size += 1 + 4;
      }
      if (Ry != 0F) {
        size += 1 + 4;
      }
      if (Rz != 0F) {
        size += 1 + 4;
      }
      if (Speed != 0F) {
        size += 1 + 4;
      }
      if (Health != 0F) {
        size += 1 + 4;
      }
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public void MergeFrom(PlayerInfo other) {
      if (other == null) {
        return;
      }
      if (other.PlayerId.Length != 0) {
        PlayerId = other.PlayerId;
      }
      if (other.X != 0F) {
        X = other.X;
      }
      if (other.Y != 0F) {
        Y = other.Y;
      }
      if (other.Z != 0F) {
        Z = other.Z;
      }
      if (other.Rx != 0F) {
        Rx = other.Rx;
      }
      if (other.Ry != 0F) {
        Ry = other.Ry;
      }
      if (other.Rz != 0F) {
        Rz = other.Rz;
      }
      if (other.Speed != 0F) {
        Speed = other.Speed;
      }
      if (other.Health != 0F) {
        Health = other.Health;
      }
      _unknownFields = pb::UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public void MergeFrom(pb::CodedInputStream input) {
    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      input.ReadRawMessage(this);
    #else
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
      if ((tag & 7) == 4) {
        // Abort on any end group tag.
        return;
      }
      switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
            break;
          case 10: {
            PlayerId = input.ReadString();
            break;
          }
          case 21: {
            X = input.ReadFloat();
            break;
          }
          case 29: {
            Y = input.ReadFloat();
            break;
          }
          case 37: {
            Z = input.ReadFloat();
            break;
          }
          case 45: {
            Rx = input.ReadFloat();
            break;
          }
          case 53: {
            Ry = input.ReadFloat();
            break;
          }
          case 61: {
            Rz = input.ReadFloat();
            break;
          }
          case 69: {
            Speed = input.ReadFloat();
            break;
          }
          case 77: {
            Health = input.ReadFloat();
            break;
          }
        }
      }
    #endif
    }

    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    void pb::IBufferMessage.InternalMergeFrom(ref pb::ParseContext input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
      if ((tag & 7) == 4) {
        // Abort on any end group tag.
        return;
      }
      switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, ref input);
            break;
          case 10: {
            PlayerId = input.ReadString();
            break;
          }
          case 21: {
            X = input.ReadFloat();
            break;
          }
          case 29: {
            Y = input.ReadFloat();
            break;
          }
          case 37: {
            Z = input.ReadFloat();
            break;
          }
          case 45: {
            Rx = input.ReadFloat();
            break;
          }
          case 53: {
            Ry = input.ReadFloat();
            break;
          }
          case 61: {
            Rz = input.ReadFloat();
            break;
          }
          case 69: {
            Speed = input.ReadFloat();
            break;
          }
          case 77: {
            Health = input.ReadFloat();
            break;
          }
        }
      }
    }
    #endif

  }

  #endregion

}

#endregion Designer generated code
