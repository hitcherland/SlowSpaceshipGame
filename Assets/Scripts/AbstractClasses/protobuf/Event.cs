// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: Event.proto
// </auto-generated>
#pragma warning disable 1591, 0612, 3021
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using pbr = global::Google.Protobuf.Reflection;
using scg = global::System.Collections.Generic;
namespace GameProtobufs {

  /// <summary>Holder for reflection information generated from Event.proto</summary>
  public static partial class EventReflection {

    #region Descriptor
    /// <summary>File descriptor for Event.proto</summary>
    public static pbr::FileDescriptor Descriptor {
      get { return descriptor; }
    }
    private static pbr::FileDescriptor descriptor;

    static EventReflection() {
      byte[] descriptorData = global::System.Convert.FromBase64String(
          string.Concat(
            "CgtFdmVudC5wcm90bxINR2FtZVByb3RvYnVmcxoPdHJhbnNmb3JtLnByb3Rv",
            "IpIBCg9DbGllbnRKb2luRXZlbnQSEQoJdGltZXN0YW1wGAEgASgFEjIKCXRy",
            "YW5zZm9ybRgCIAEoCzIfLkdhbWVQcm90b2J1ZnMuVHJhbnNmb3JtTWVzc2Fn",
            "ZRIWCg5tb2RlbEFzc2V0UGF0aBgDIAEoCRIMCgR0ZWFtGAQgASgFEhIKCnlv",
            "dU93blRoaXMYBSABKAhiBnByb3RvMw=="));
      descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
          new pbr::FileDescriptor[] { global::GameProtobufs.TransformReflection.Descriptor, },
          new pbr::GeneratedClrTypeInfo(null, null, new pbr::GeneratedClrTypeInfo[] {
            new pbr::GeneratedClrTypeInfo(typeof(global::GameProtobufs.ClientJoinEvent), global::GameProtobufs.ClientJoinEvent.Parser, new[]{ "Timestamp", "Transform", "ModelAssetPath", "Team", "YouOwnThis" }, null, null, null, null)
          }));
    }
    #endregion

  }
  #region Messages
  public sealed partial class ClientJoinEvent : pb::IMessage<ClientJoinEvent> {
    private static readonly pb::MessageParser<ClientJoinEvent> _parser = new pb::MessageParser<ClientJoinEvent>(() => new ClientJoinEvent());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<ClientJoinEvent> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::GameProtobufs.EventReflection.Descriptor.MessageTypes[0]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public ClientJoinEvent() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public ClientJoinEvent(ClientJoinEvent other) : this() {
      timestamp_ = other.timestamp_;
      transform_ = other.transform_ != null ? other.transform_.Clone() : null;
      modelAssetPath_ = other.modelAssetPath_;
      team_ = other.team_;
      youOwnThis_ = other.youOwnThis_;
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public ClientJoinEvent Clone() {
      return new ClientJoinEvent(this);
    }

    /// <summary>Field number for the "timestamp" field.</summary>
    public const int TimestampFieldNumber = 1;
    private int timestamp_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int Timestamp {
      get { return timestamp_; }
      set {
        timestamp_ = value;
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

    /// <summary>Field number for the "modelAssetPath" field.</summary>
    public const int ModelAssetPathFieldNumber = 3;
    private string modelAssetPath_ = "";
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string ModelAssetPath {
      get { return modelAssetPath_; }
      set {
        modelAssetPath_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "team" field.</summary>
    public const int TeamFieldNumber = 4;
    private int team_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int Team {
      get { return team_; }
      set {
        team_ = value;
      }
    }

    /// <summary>Field number for the "youOwnThis" field.</summary>
    public const int YouOwnThisFieldNumber = 5;
    private bool youOwnThis_;
    /// <summary>
    ///ShipMessage ship = 6;
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool YouOwnThis {
      get { return youOwnThis_; }
      set {
        youOwnThis_ = value;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as ClientJoinEvent);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(ClientJoinEvent other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (Timestamp != other.Timestamp) return false;
      if (!object.Equals(Transform, other.Transform)) return false;
      if (ModelAssetPath != other.ModelAssetPath) return false;
      if (Team != other.Team) return false;
      if (YouOwnThis != other.YouOwnThis) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (Timestamp != 0) hash ^= Timestamp.GetHashCode();
      if (transform_ != null) hash ^= Transform.GetHashCode();
      if (ModelAssetPath.Length != 0) hash ^= ModelAssetPath.GetHashCode();
      if (Team != 0) hash ^= Team.GetHashCode();
      if (YouOwnThis != false) hash ^= YouOwnThis.GetHashCode();
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
      if (Timestamp != 0) {
        output.WriteRawTag(8);
        output.WriteInt32(Timestamp);
      }
      if (transform_ != null) {
        output.WriteRawTag(18);
        output.WriteMessage(Transform);
      }
      if (ModelAssetPath.Length != 0) {
        output.WriteRawTag(26);
        output.WriteString(ModelAssetPath);
      }
      if (Team != 0) {
        output.WriteRawTag(32);
        output.WriteInt32(Team);
      }
      if (YouOwnThis != false) {
        output.WriteRawTag(40);
        output.WriteBool(YouOwnThis);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (Timestamp != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(Timestamp);
      }
      if (transform_ != null) {
        size += 1 + pb::CodedOutputStream.ComputeMessageSize(Transform);
      }
      if (ModelAssetPath.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(ModelAssetPath);
      }
      if (Team != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(Team);
      }
      if (YouOwnThis != false) {
        size += 1 + 1;
      }
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(ClientJoinEvent other) {
      if (other == null) {
        return;
      }
      if (other.Timestamp != 0) {
        Timestamp = other.Timestamp;
      }
      if (other.transform_ != null) {
        if (transform_ == null) {
          Transform = new global::GameProtobufs.TransformMessage();
        }
        Transform.MergeFrom(other.Transform);
      }
      if (other.ModelAssetPath.Length != 0) {
        ModelAssetPath = other.ModelAssetPath;
      }
      if (other.Team != 0) {
        Team = other.Team;
      }
      if (other.YouOwnThis != false) {
        YouOwnThis = other.YouOwnThis;
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
          case 8: {
            Timestamp = input.ReadInt32();
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
            ModelAssetPath = input.ReadString();
            break;
          }
          case 32: {
            Team = input.ReadInt32();
            break;
          }
          case 40: {
            YouOwnThis = input.ReadBool();
            break;
          }
        }
      }
    }

  }

  #endregion

}

#endregion Designer generated code
