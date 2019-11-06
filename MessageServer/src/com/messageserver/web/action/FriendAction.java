package com.messageserver.web.action;

import com.messageserver.domain.Friend;
import com.messageserver.domain.User;
import com.messageserver.service.FriendService;
import com.messageserver.service.UserService;
import com.messageserver.utils.ServiceFactory;
import com.messageserver.utils.SessionThreadLocal;
import com.opensymphony.xwork2.ActionSupport;
import org.apache.struts2.ServletActionContext;

import javax.servlet.http.HttpServletResponse;
import javax.servlet.http.HttpSession;
import java.io.IOException;
import java.util.Date;
import java.util.List;

public class FriendAction extends ActionSupport {

    private FriendService friendService = (FriendService) ServiceFactory.newServiceInstance("FriendService");
    private UserService userService = (UserService) ServiceFactory.newServiceInstance("UserService");

    public String getNewFriend() throws IOException {
        SessionThreadLocal.openSession();
        HttpSession session = ServletActionContext.getRequest().getSession();
        HttpServletResponse response = ServletActionContext.getResponse();
        User user = (User) session.getAttribute("user");
        if(user == null){
            response.getWriter().write("failed:5");
        }else{
            List<Friend> allFriend = friendService.getAllFriend(user);
            StringBuffer sb = new StringBuffer();
            sb.append("<friend>");
            for(Friend friend : allFriend){
                if(friend.getStatus() == 1){
                    User fuser = userService.getUserById(friend.getFriendId());
                    sb.append(fuser.toXML());
                    friend.setStatus(0);
                    friendService.saveChange(friend);
                }
            }
            sb.append("</friend>");
            response.getWriter().write(sb.toString());
        }
        SessionThreadLocal.closeSession();
        return NONE;
    }

    private String username;
    private String id;

    public String searchFriend() throws IOException {
        SessionThreadLocal.openSession();
        HttpServletResponse response = ServletActionContext.getResponse();
        User user = userService.getUserByUsername(username);
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
        SessionThreadLocal.closeSession();
        return NONE;
    }

    public String addFriend() throws IOException {
        SessionThreadLocal.openSession();
        HttpSession session = ServletActionContext.getRequest().getSession();
        HttpServletResponse response = ServletActionContext.getResponse();
        User user = (User) session.getAttribute("user");
        if(user == null){
            response.getWriter().write("failed:6");
        }else{
            User target = userService.getUserById(id);
            if(target == null){
                response.getWriter().write("failed:7");
            }else{
                Friend friend = friendService.getFriend(user.getId(), target.getId());
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
                    userService.changeStatus(target, 1 | target.getStatus());
                    friendService.saveFriend(f1);
                    friendService.saveFriend(f2);
                    response.getWriter().write("success");
                }else{
                    response.getWriter().write("repeat");
                }
            }
        }
        SessionThreadLocal.closeSession();
        return NONE;
    }

    public String getUsername() {
        return username;
    }

    public void setUsername(String username) {
        this.username = username;
    }

    public String getId() {
        return id;
    }

    public void setId(String id) {
        this.id = id;
    }
}
