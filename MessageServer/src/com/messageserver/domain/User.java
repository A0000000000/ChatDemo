package com.messageserver.domain;

import java.io.Serializable;
import java.util.Date;

public class User implements Serializable {

    public String getId() {
        return id;
    }

    public void setId(String id) {
        this.id = id;
    }

    public String getUsername() {
        return username;
    }

    public void setUsername(String username) {
        this.username = username;
    }

    public String getPassword() {
        return password;
    }

    public void setPassword(String password) {
        this.password = password;
    }

    public Date getBirthday() {
        return birthday;
    }

    public void setBirthday(Date birthday) {
        this.birthday = birthday;
    }

    public String getEmail() {
        return email;
    }

    public void setEmail(String email) {
        this.email = email;
    }

    public String getPhone() {
        return phone;
    }

    public void setPhone(String phone) {
        this.phone = phone;
    }

    public Integer getStatus() {
        return status;
    }

    public void setStatus(Integer status) {
        this.status = status;
    }

    public String toXML(){
        StringBuffer sb = new StringBuffer();
        sb.append("<user>");
        sb.append("<id>"+ id +"</id>");
        sb.append("<username>"+ username +"</username>");
        sb.append("<birthday>"+ (birthday == null ? "" : birthday.toString()) +"</birthday>");
        sb.append("<email>"+ email +"</email>");
        sb.append("<phone>"+ (phone == null ? "" : phone) +"</phone>");
        sb.append("</user>");
        return sb.toString();
    }

    private String id;
    private String username;
    private String password;
    private Date birthday;
    private String email;
    private String phone;
    private Integer status;

}
