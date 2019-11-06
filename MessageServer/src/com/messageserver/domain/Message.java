package com.messageserver.domain;

import java.io.Serializable;
import java.util.Date;

public class Message implements Serializable {

    public String getId() {
        return id;
    }

    public void setId(String id) {
        this.id = id;
    }

    public String getSenderId() {
        return senderId;
    }

    public void setSenderId(String senderId) {
        this.senderId = senderId;
    }

    public String getReceiverId() {
        return receiverId;
    }

    public void setReceiverId(String receiverId) {
        this.receiverId = receiverId;
    }

    public String getContent() {
        return content;
    }

    public void setContent(String content) {
        this.content = content;
    }

    public Date getSendTime() {
        return sendTime;
    }

    public void setSendTime(Date sendTime) {
        this.sendTime = sendTime;
    }

    public Integer getStatus() {
        return status;
    }

    public void setStatus(Integer status) {
        this.status = status;
    }

    public String toJSON(){
        StringBuffer sb = new StringBuffer();
        sb.append("{");
        sb.append("\"id\" : \"" + id + "\",");
        sb.append("\"senderId\" : \"" + senderId + "\",");
        sb.append("\"receiverId\" : \"" + receiverId + "\",");
        sb.append("\"content\" : \"" + content + "\",");
        sb.append("\"sendTime\" : \"" + sendTime.toString() + "\"");
        sb.append("}");
        return sb.toString();
    }

    private String id;
    private String senderId;
    private String receiverId;
    private String content;
    private Date sendTime;
    private Integer status;
}
