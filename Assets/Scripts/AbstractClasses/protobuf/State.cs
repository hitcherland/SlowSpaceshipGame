// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: State.proto
// </auto-generated>
#pragma warning disable 1591, 0612, 3021
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using pbr = global::Google.Protobuf.Reflection;
using scg = global::System.Collections.Generic;
namespace GameProtobufs {

  /// <summary>Holder for reflection information generated from State.proto</summary>
  public static partial class StateReflection {

    #region Descriptor
    /// <summary>File descriptor for State.proto</summary>
    public static pbr::FileDescriptor Descriptor {
      get { return descriptor; }
    }
    private static pbr::FileDescriptor descriptor;

    static StateReflection() {
      byte[] descriptorData = global::System.Convert.FromBase64String(
          string.Concat(
            "CgtTdGF0ZS5wcm90bxINR2FtZVByb3RvYnVmcxoPdHJhbnNmb3JtLnByb3Rv",
            "Im8KC1NoaXBNZXNzYWdlEgoKAmlkGAEgASgJEjIKCXRyYW5zZm9ybRgCIAEo",
            "CzIfLkdhbWVQcm90b2J1ZnMuVHJhbnNmb3JtTWVzc2FnZRIRCglvd25lckd1",
            "aWQYAyABKAkSDQoFbW9kZWwYBCABKAkiOQoMU3RhdGVNZXNzYWdlEikKBXNo",
            "aXBzGAEgAygLMhouR2FtZVByb3RvYnVmcy5TaGlwTWVzc2FnZSoyCghTaGlw",
            "VHlwZRILCgdjYXBpdGFsEAASCwoHZmlnaHRlchABEgwKCGVuZ2luZWVyEAJi",
            "BnByb3RvMw=="));
      descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
          new pbr::FileDescriptor[] { global::GameProtobufs.TransformReflection.Descriptor, },
          new pbr::GeneratedClrTypeInfo(new[] {typeof(global::GameProtobufs.ShipType), }, null, new pbr::GeneratedClrTypeInfo[] {
            new pbr::GeneratedClrTypeInfo(typeof(global::GameProtobufs.ShipMessage), global::GameProtobufs.ShipMessage.Parser, new[]{ "Id", "Transform", "OwnerGuid", "Model" }, null, null, null, null),
            new pbr::GeneratedClrTypeInfo(typeof(global::GameProtobufs.StateMessage), global::GameProtobufs.StateMessage.Parser, new[]{ "Ships" }, null, null, null, null)
          }));
    }
    #endregion

  }
  #region Enums
  public enum ShipType {
    [pbr::OriginalName("capital")] Capital = 0,
    [pbr::OriginalName("fighter")] Fighter = 1,
    [pbr::OriginalName("engineer")] Engineer = 2,
  }

  #endregion

  #region Messages
  public sealed partial class ShipMessage : pb::IMessage<ShipMessage> {
    private static readonly pb::MessageParser<ShipMessage> _parser = new pb::MessageParser<ShipMessage>(() => new ShipMessage());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<ShipMessage> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::GameProtobufs.StateReflection.Descriptor.MessageTypes[0]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public ShipMessage() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public ShipMessage(ShipMessage other) : this() {
      id_ = other.id_;
      transform_ = other.transform_ != null ? other.transform_.Clone() : null;
      ownerGuid_ = other.ownerGuid_;
      model_ = other.model_;
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public ShipMessage Clone() {
      return new ShipMessage(this);
    }

    /// <summary>Field number for the "id" field.</summary>
    public const int IdFieldNumber = 1;
    private string id_ = "";
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string Id {
      get { return id_; }
      set {
        id_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "transform" field.</summary>
    public const int TransformFieldNumber = 2;
    private global::GameProtobufs.TransformMessage transform_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public global::GameProtobufs.TransformMessage Transform {
      get { return transform_; }
      set {
        transform_ = value;
      }
    }

    /// <summary>Field number for the "ownerGuid" field.</summary>
    public const int OwnerGuidFieldNumber = 3;
    private string ownerGuid_ = "";
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string OwnerGuid {
      get { return ownerGuid_; }
      set {
        ownerGuid_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "model" field.</summary>
    public const int ModelFieldNumber = 4;
    private string model_ = "";
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string Model {
      get { return model_; }
      set {
        model_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as ShipMessage);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(ShipMessage other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (Id != other.Id) return false;
      if (!object.Equals(Transform, other.Transform)) return false;
      if (OwnerGuid != other.OwnerGuid) return false;
      if (Model != other.Model) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (Id.Length != 0) hash ^= Id.GetHashCode();
      if (transform_ != null) hash ^= Transform.GetHashCode();
      if (OwnerGuid.Length != 0) hash ^= OwnerGuid.GetHashCode();
      if (Model.Length != 0) hash ^= Model.GetHashCode();
      if (_unknownFields != null) {
        hash ^= _unknownFields.GetHashCode();
      }
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void WriteTo(pb::CodedOutputStream output) {
      if (Id.Length != 0) {
        output.WriteRawTag(10);
        output.WriteString(Id);
      }
      if (transform_ != null) {
        output.WriteRawTag(18);
        output.WriteMessage(Transform);
      }
      if (OwnerGuid.Length != 0) {
        output.WriteRawTag(26);
        output.WriteString(OwnerGuid);
      }
      if (Model.Length != 0) {
        output.WriteRawTag(34);
        output.WriteString(Model);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (Id.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(Id);
      }
      if (transform_ != null) {
        size += 1 + pb::CodedOutputStream.ComputeMessageSize(Transform);
      }
      if (OwnerGuid.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(OwnerGuid);
      }
      if (Model.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(Model);
      }
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(ShipMessage other) {
      if (other == null) {
        return;
      }
      if (other.Id.Length != 0) {
        Id = other.Id;
      }
      if (other.transform_ != null) {
        if (transform_ == null) {
          Transform = new global::GameProtobufs.TransformMessage();
        }
        Transform.MergeFrom(other.Transform);
      }
      if (other.OwnerGuid.Length != 0) {
        OwnerGuid = other.OwnerGuid;
      }
      if (other.Model.Length != 0) {
        Model = other.Model;
      }
      _unknownFields = pb::UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(pb::CodedInputStream input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
            break;
          case 10: {
            Id = input.ReadString();
            break;
          }
          case 18: {
            if (transform_ == null) {
              Transform = new global::GameProtobufs.TransformMessage();
            }
            input.ReadMessage(Transform);
            break;
          }
          case 26: {
            OwnerGuid = input.ReadString();
            break;
          }
          case 34: {
            Model = input.ReadString();
            break;
          }
        }
      }
    }

  }

  public sealed partial class StateMessage : pb::IMessage<StateMessage> {
    private static readonly pb::MessageParser<StateMessage> _parser = new pb::MessageParser<StateMessage>(() => new StateMessage());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<StateMessage> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::GameProtobufs.StateReflection.Descriptor.MessageTypes[1]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public StateMessage() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public StateMessage(StateMessage other) : this() {
      ships_ = other.ships_.Clone();
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public StateMessage Clone() {
      return new StateMessage(this);
    }

    /// <summary>Field number for the "ships" field.</summary>
    public const int ShipsFieldNumber = 1;
    private static readonly pb::FieldCodec<global::GameProtobufs.ShipMessage> _repeated_ships_codec
        = pb::FieldCodec.ForMessage(10, global::GameProtobufs.ShipMessage.Parser);
    private readonly pbc::RepeatedField<global::GameProtobufs.ShipMessage> ships_ = new pbc::RepeatedField<global::GameProtobufs.ShipMessage>();
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public pbc::RepeatedField<global::GameProtobufs.ShipMessage> Ships {
      get { return ships_; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as StateMessage);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(StateMessage other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if(!ships_.Equals(other.ships_)) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      hash ^= ships_.GetHashCode();
      if (_unknownFields != null) {
        hash ^= _unknownFields.GetHashCode();
      }
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void WriteTo(pb::CodedOutputStream output) {
      ships_.WriteTo(output, _repeated_ships_codec);
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      size += ships_.CalculateSize(_repeated_ships_codec);
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(StateMessage other) {
      if (other == null) {
        return;
      }
      ships_.Add(other.ships_);
      _unknownFields = pb::UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(pb::CodedInputStream input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
            break;
          case 10: {
            ships_.AddEntriesFrom(input, _repeated_ships_codec);
            break;
          }
        }
      }
    }

  }

  #endregion

}

#endregion Designer generated code
