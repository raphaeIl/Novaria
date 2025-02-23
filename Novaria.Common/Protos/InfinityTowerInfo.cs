// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: infinity_tower_info.proto
// </auto-generated>
#pragma warning disable 1591, 0612, 3021, 8981
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using pbr = global::Google.Protobuf.Reflection;
using scg = global::System.Collections.Generic;
namespace Proto {

  /// <summary>Holder for reflection information generated from infinity_tower_info.proto</summary>
  public static partial class InfinityTowerInfoReflection {

    #region Descriptor
    /// <summary>File descriptor for infinity_tower_info.proto</summary>
    public static pbr::FileDescriptor Descriptor {
      get { return descriptor; }
    }
    private static pbr::FileDescriptor descriptor;

    static InfinityTowerInfoReflection() {
      byte[] descriptorData = global::System.Convert.FromBase64String(
          string.Concat(
            "ChlpbmZpbml0eV90b3dlcl9pbmZvLnByb3RvEgVwcm90bxoMcHVibGljLnBy",
            "b3RvIoIBChVJbmZpbml0eVRvd2VySW5mb1Jlc3ASEwoLQm91bnR5TGV2ZWwY",
            "ASABKA0SEAoIUGxvdHNJZHMYAiADKA0SLAoFSW5mb3MYAyADKAsyHS5wcm90",
            "by5JbmZpbml0eVRvd2VyTGV2ZWxJbmZvEhQKC05leHRQYWNrYWdlGP8PIAEo",
            "DFAAYgZwcm90bzM="));
      descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
          new pbr::FileDescriptor[] { global::Proto.PublicReflection.Descriptor, },
          new pbr::GeneratedClrTypeInfo(null, null, new pbr::GeneratedClrTypeInfo[] {
            new pbr::GeneratedClrTypeInfo(typeof(global::Proto.InfinityTowerInfoResp), global::Proto.InfinityTowerInfoResp.Parser, new[]{ "BountyLevel", "PlotsIds", "Infos", "NextPackage" }, null, null, null, null)
          }));
    }
    #endregion

  }
  #region Messages
  [global::System.Diagnostics.DebuggerDisplayAttribute("{ToString(),nq}")]
  public sealed partial class InfinityTowerInfoResp : pb::IMessage<InfinityTowerInfoResp>
  #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      , pb::IBufferMessage
  #endif
  {
    private static readonly pb::MessageParser<InfinityTowerInfoResp> _parser = new pb::MessageParser<InfinityTowerInfoResp>(() => new InfinityTowerInfoResp());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public static pb::MessageParser<InfinityTowerInfoResp> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Proto.InfinityTowerInfoReflection.Descriptor.MessageTypes[0]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public InfinityTowerInfoResp() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public InfinityTowerInfoResp(InfinityTowerInfoResp other) : this() {
      bountyLevel_ = other.bountyLevel_;
      plotsIds_ = other.plotsIds_.Clone();
      infos_ = other.infos_.Clone();
      nextPackage_ = other.nextPackage_;
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public InfinityTowerInfoResp Clone() {
      return new InfinityTowerInfoResp(this);
    }

    /// <summary>Field number for the "BountyLevel" field.</summary>
    public const int BountyLevelFieldNumber = 1;
    private uint bountyLevel_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public uint BountyLevel {
      get { return bountyLevel_; }
      set {
        bountyLevel_ = value;
      }
    }

    /// <summary>Field number for the "PlotsIds" field.</summary>
    public const int PlotsIdsFieldNumber = 2;
    private static readonly pb::FieldCodec<uint> _repeated_plotsIds_codec
        = pb::FieldCodec.ForUInt32(18);
    private readonly pbc::RepeatedField<uint> plotsIds_ = new pbc::RepeatedField<uint>();
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public pbc::RepeatedField<uint> PlotsIds {
      get { return plotsIds_; }
    }

    /// <summary>Field number for the "Infos" field.</summary>
    public const int InfosFieldNumber = 3;
    private static readonly pb::FieldCodec<global::Proto.InfinityTowerLevelInfo> _repeated_infos_codec
        = pb::FieldCodec.ForMessage(26, global::Proto.InfinityTowerLevelInfo.Parser);
    private readonly pbc::RepeatedField<global::Proto.InfinityTowerLevelInfo> infos_ = new pbc::RepeatedField<global::Proto.InfinityTowerLevelInfo>();
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public pbc::RepeatedField<global::Proto.InfinityTowerLevelInfo> Infos {
      get { return infos_; }
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
      return Equals(other as InfinityTowerInfoResp);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public bool Equals(InfinityTowerInfoResp other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (BountyLevel != other.BountyLevel) return false;
      if(!plotsIds_.Equals(other.plotsIds_)) return false;
      if(!infos_.Equals(other.infos_)) return false;
      if (NextPackage != other.NextPackage) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override int GetHashCode() {
      int hash = 1;
      if (BountyLevel != 0) hash ^= BountyLevel.GetHashCode();
      hash ^= plotsIds_.GetHashCode();
      hash ^= infos_.GetHashCode();
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
      if (BountyLevel != 0) {
        output.WriteRawTag(8);
        output.WriteUInt32(BountyLevel);
      }
      plotsIds_.WriteTo(output, _repeated_plotsIds_codec);
      infos_.WriteTo(output, _repeated_infos_codec);
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
      if (BountyLevel != 0) {
        output.WriteRawTag(8);
        output.WriteUInt32(BountyLevel);
      }
      plotsIds_.WriteTo(ref output, _repeated_plotsIds_codec);
      infos_.WriteTo(ref output, _repeated_infos_codec);
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
      if (BountyLevel != 0) {
        size += 1 + pb::CodedOutputStream.ComputeUInt32Size(BountyLevel);
      }
      size += plotsIds_.CalculateSize(_repeated_plotsIds_codec);
      size += infos_.CalculateSize(_repeated_infos_codec);
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
    public void MergeFrom(InfinityTowerInfoResp other) {
      if (other == null) {
        return;
      }
      if (other.BountyLevel != 0) {
        BountyLevel = other.BountyLevel;
      }
      plotsIds_.Add(other.plotsIds_);
      infos_.Add(other.infos_);
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
          case 8: {
            BountyLevel = input.ReadUInt32();
            break;
          }
          case 18:
          case 16: {
            plotsIds_.AddEntriesFrom(input, _repeated_plotsIds_codec);
            break;
          }
          case 26: {
            infos_.AddEntriesFrom(input, _repeated_infos_codec);
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
          case 8: {
            BountyLevel = input.ReadUInt32();
            break;
          }
          case 18:
          case 16: {
            plotsIds_.AddEntriesFrom(ref input, _repeated_plotsIds_codec);
            break;
          }
          case 26: {
            infos_.AddEntriesFrom(ref input, _repeated_infos_codec);
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
