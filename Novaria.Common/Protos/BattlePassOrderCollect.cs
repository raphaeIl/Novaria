// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: battle_pass_order_collect.proto
// </auto-generated>
#pragma warning disable 1591, 0612, 3021, 8981
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using pbr = global::Google.Protobuf.Reflection;
using scg = global::System.Collections.Generic;
namespace Proto {

  /// <summary>Holder for reflection information generated from battle_pass_order_collect.proto</summary>
  public static partial class BattlePassOrderCollectReflection {

    #region Descriptor
    /// <summary>File descriptor for battle_pass_order_collect.proto</summary>
    public static pbr::FileDescriptor Descriptor {
      get { return descriptor; }
    }
    private static pbr::FileDescriptor descriptor;

    static BattlePassOrderCollectReflection() {
      byte[] descriptorData = global::System.Convert.FromBase64String(
          string.Concat(
            "Ch9iYXR0bGVfcGFzc19vcmRlcl9jb2xsZWN0LnByb3RvEgVwcm90bxoMcHVi",
            "bGljLnByb3RvIokBChpCYXR0bGVQYXNzT3JkZXJDb2xsZWN0UmVzcBInCgtD",
            "b2xsZWN0UmVzcBgBIAEoCzISLnByb3RvLkNvbGxlY3RSZXNwEgwKBE1vZGUY",
            "AiABKA0SDQoFTGV2ZWwYAyABKA0SDwoHVmVyc2lvbhgEIAEoDRIUCgtOZXh0",
            "UGFja2FnZRj/DyABKAxQAGIGcHJvdG8z"));
      descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
          new pbr::FileDescriptor[] { global::Proto.PublicReflection.Descriptor, },
          new pbr::GeneratedClrTypeInfo(null, null, new pbr::GeneratedClrTypeInfo[] {
            new pbr::GeneratedClrTypeInfo(typeof(global::Proto.BattlePassOrderCollectResp), global::Proto.BattlePassOrderCollectResp.Parser, new[]{ "CollectResp", "Mode", "Level", "Version", "NextPackage" }, null, null, null, null)
          }));
    }
    #endregion

  }
  #region Messages
  [global::System.Diagnostics.DebuggerDisplayAttribute("{ToString(),nq}")]
  public sealed partial class BattlePassOrderCollectResp : pb::IMessage<BattlePassOrderCollectResp>
  #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      , pb::IBufferMessage
  #endif
  {
    private static readonly pb::MessageParser<BattlePassOrderCollectResp> _parser = new pb::MessageParser<BattlePassOrderCollectResp>(() => new BattlePassOrderCollectResp());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public static pb::MessageParser<BattlePassOrderCollectResp> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Proto.BattlePassOrderCollectReflection.Descriptor.MessageTypes[0]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public BattlePassOrderCollectResp() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public BattlePassOrderCollectResp(BattlePassOrderCollectResp other) : this() {
      collectResp_ = other.collectResp_ != null ? other.collectResp_.Clone() : null;
      mode_ = other.mode_;
      level_ = other.level_;
      version_ = other.version_;
      nextPackage_ = other.nextPackage_;
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public BattlePassOrderCollectResp Clone() {
      return new BattlePassOrderCollectResp(this);
    }

    /// <summary>Field number for the "CollectResp" field.</summary>
    public const int CollectRespFieldNumber = 1;
    private global::Proto.CollectResp collectResp_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public global::Proto.CollectResp CollectResp {
      get { return collectResp_; }
      set {
        collectResp_ = value;
      }
    }

    /// <summary>Field number for the "Mode" field.</summary>
    public const int ModeFieldNumber = 2;
    private uint mode_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public uint Mode {
      get { return mode_; }
      set {
        mode_ = value;
      }
    }

    /// <summary>Field number for the "Level" field.</summary>
    public const int LevelFieldNumber = 3;
    private uint level_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public uint Level {
      get { return level_; }
      set {
        level_ = value;
      }
    }

    /// <summary>Field number for the "Version" field.</summary>
    public const int VersionFieldNumber = 4;
    private uint version_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public uint Version {
      get { return version_; }
      set {
        version_ = value;
      }
    }

    /// <summary>Field number for the "NextPackage" field.</summary>
    public const int NextPackageFieldNumber = 2047;
    private pb::ByteString nextPackage_ = pb::ByteString.Empty;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public pb::ByteString NextPackage {
      get { return nextPackage_; }
      set {
        nextPackage_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override bool Equals(object other) {
      return Equals(other as BattlePassOrderCollectResp);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public bool Equals(BattlePassOrderCollectResp other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (!object.Equals(CollectResp, other.CollectResp)) return false;
      if (Mode != other.Mode) return false;
      if (Level != other.Level) return false;
      if (Version != other.Version) return false;
      if (NextPackage != other.NextPackage) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override int GetHashCode() {
      int hash = 1;
      if (collectResp_ != null) hash ^= CollectResp.GetHashCode();
      if (Mode != 0) hash ^= Mode.GetHashCode();
      if (Level != 0) hash ^= Level.GetHashCode();
      if (Version != 0) hash ^= Version.GetHashCode();
      if (NextPackage.Length != 0) hash ^= NextPackage.GetHashCode();
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
      if (collectResp_ != null) {
        output.WriteRawTag(10);
        output.WriteMessage(CollectResp);
      }
      if (Mode != 0) {
        output.WriteRawTag(16);
        output.WriteUInt32(Mode);
      }
      if (Level != 0) {
        output.WriteRawTag(24);
        output.WriteUInt32(Level);
      }
      if (Version != 0) {
        output.WriteRawTag(32);
        output.WriteUInt32(Version);
      }
      if (NextPackage.Length != 0) {
        output.WriteRawTag(250, 127);
        output.WriteBytes(NextPackage);
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
      if (collectResp_ != null) {
        output.WriteRawTag(10);
        output.WriteMessage(CollectResp);
      }
      if (Mode != 0) {
        output.WriteRawTag(16);
        output.WriteUInt32(Mode);
      }
      if (Level != 0) {
        output.WriteRawTag(24);
        output.WriteUInt32(Level);
      }
      if (Version != 0) {
        output.WriteRawTag(32);
        output.WriteUInt32(Version);
      }
      if (NextPackage.Length != 0) {
        output.WriteRawTag(250, 127);
        output.WriteBytes(NextPackage);
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
      if (collectResp_ != null) {
        size += 1 + pb::CodedOutputStream.ComputeMessageSize(CollectResp);
      }
      if (Mode != 0) {
        size += 1 + pb::CodedOutputStream.ComputeUInt32Size(Mode);
      }
      if (Level != 0) {
        size += 1 + pb::CodedOutputStream.ComputeUInt32Size(Level);
      }
      if (Version != 0) {
        size += 1 + pb::CodedOutputStream.ComputeUInt32Size(Version);
      }
      if (NextPackage.Length != 0) {
        size += 2 + pb::CodedOutputStream.ComputeBytesSize(NextPackage);
      }
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public void MergeFrom(BattlePassOrderCollectResp other) {
      if (other == null) {
        return;
      }
      if (other.collectResp_ != null) {
        if (collectResp_ == null) {
          CollectResp = new global::Proto.CollectResp();
        }
        CollectResp.MergeFrom(other.CollectResp);
      }
      if (other.Mode != 0) {
        Mode = other.Mode;
      }
      if (other.Level != 0) {
        Level = other.Level;
      }
      if (other.Version != 0) {
        Version = other.Version;
      }
      if (other.NextPackage.Length != 0) {
        NextPackage = other.NextPackage;
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
            if (collectResp_ == null) {
              CollectResp = new global::Proto.CollectResp();
            }
            input.ReadMessage(CollectResp);
            break;
          }
          case 16: {
            Mode = input.ReadUInt32();
            break;
          }
          case 24: {
            Level = input.ReadUInt32();
            break;
          }
          case 32: {
            Version = input.ReadUInt32();
            break;
          }
          case 16378: {
            NextPackage = input.ReadBytes();
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
            if (collectResp_ == null) {
              CollectResp = new global::Proto.CollectResp();
            }
            input.ReadMessage(CollectResp);
            break;
          }
          case 16: {
            Mode = input.ReadUInt32();
            break;
          }
          case 24: {
            Level = input.ReadUInt32();
            break;
          }
          case 32: {
            Version = input.ReadUInt32();
            break;
          }
          case 16378: {
            NextPackage = input.ReadBytes();
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
