package com.messageserver.web.action;

import com.messageserver.domain.Friend;
import com.messageserver.domain.User;
import org.apache.struts2.ServletActionContext;

import javax.servlet.http.HttpServletResponse;
import javax.servlet.http.HttpSession;
import java.io.IOException;
import java.util.Date;
import java.util.List;

public class FriendAction extends BaseAction<Friend> {

    public String getNewFriend() throws IOException {
        HttpSession session = ServletActionContext.getRequest().getSession();
        HttpServletResponse response = ServletActionContext.getResponse();
        User user = (User) session.getAttribute("user");
        if(user == null){
            response.getWriter().write("failed:5");
        }else{
            List<Friend> allFriend = getFriendService().getAllFriend(user);
            StringBuffer sb = new StringBuffer();
            sb.append("<friend>");
            for(Friend friend : allFriend){
                if(friend.getStatus() == 1){
                    User fuser = getUserService().getUserById(friend.getFriendId());
                    sb.append(fuser.toXML());
                    friend.setStatus(0);
                    getFriendService().saveChange(friend);
                }
            }
            sb.append("</friend>");
            response.getWriter().write(sb.toString());
        }
        return NONE;
    }

    private String username;

    public String searchFriend() throws IOException {
        HttpServletResponse response = ServletActionContext.getResponse();
        User user = getUserService().getUserByUsername(username);
        if (user == null){
            response.getWriter().write("none");
        }else{
            StringBuffer sb = new StringBuffer();
            sb.append("{");
            sb.append("\"id\":\"" + user.getId() + "\",");
            sb.append("\"username\":\"" + user.getUsername() + "\",");
            sb.append("\"birthday\":\"" + (user.getBirthday() == null ? "" : user.getBirthday()) + "\",");
            sb.append("\"email\":\"" + user.getEmail() + "\",");
            sb.append("\"phone\":\"" + (user.getPhone() == null ? "" : user.getPhone()) + "\"");
            sb.append("}");
            response.getWriter().write(sb.toString());
        }
        return NONE;
    }

    public String addFriend() throws IOException {
        HttpSession session = ServletActionContext.getRequest().getSession();
        HttpServletResponse response = ServletActionContext.getResponse();
        User user = (User) session.getAttribute("user");
        if(user == null){
            response.getWriter().write("failed:6");
        }else{
            User target = getUserService().getUserById(getBean().getId());
            if(target == null){
                response.getWriter().write("failed:7");
            }else{
                Friend friend = getFriendService().getFriend(user.getId(), target.getId());
                if(friend == null) {
                    Friend f1 = new Friend(), f2 = new Friend();
                    f1.setMasterId(user.getId());
                    f2.setMasterId(target.getId());
                    f1.setFriendId(target.getId());
                    f2.setFriendId(user.getId());
                    f1.setCreateTime(new Date());
                    f2.setCreateTime(new Date());
                    f1.setStatus(0);
                    f2.setStatus(1);
                    getUserService().changeStatus(target, 1 | target.getStatus());
                    getFriendService().saveFriend(f1);
                    getFriendService().saveFriend(f2);
                    response.getWriter().write("success");
                }else{
                    response.getWriter().write("repeat");
                }
            }
        }
        return NONE;
    }

    public String getUsername() {
        return username;
    }

    public void setUsername(String username) {
        this.username = username;
    }

}
