package com.messageserver.web.action;

import com.messageserver.domain.Message;
import com.messageserver.domain.User;
import com.messageserver.service.MessageService;
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

public class MessageAction extends ActionSupport {

    private String targetId;
    private String content;

    private UserService userService = (UserService) ServiceFactory.newServiceInstance("UserService");
    private MessageService messageService = (MessageService) ServiceFactory.newServiceInstance("MessageService");

    public String sendMessage() throws IOException {
        SessionThreadLocal.openSession();
        HttpSession session = ServletActionContext.getRequest().getSession();
        HttpServletResponse response = ServletActionContext.getResponse();
        User user = (User) session.getAttribute("user");
        if(user == null){
            response.getWriter().write("error:1");
        }
        Message message = new Message();
        message.setSenderId(user.getId());
        message.setReceiverId(targetId);
        message.setContent(content);
        message.setStatus(1);
        message.setSendTime(new Date());
        User target = userService.getUserById(targetId);
        userService.changeStatus(target, target.getStatus() | 2);
        boolean res = messageService.addNewMessage(message);
        if(res){
            response.getWriter().write(message.getId()+ "#" + user.getId());
        }else{
            response.getWriter().write("failed");
        }
        SessionThreadLocal.closeSession();
        return NONE;
    }

    public String getNewMessage() throws IOException {
        SessionThreadLocal.openSession();
        HttpSession session = ServletActionContext.getRequest().getSession();
        HttpServletResponse response = ServletActionContext.getResponse();
        User user = (User) session.getAttribute("user");
        if(user == null){
            response.getWriter().write("error");
        }else {
            List<Message> messages = messageService.getNewMessageByReceiverId(user.getId());
            StringBuffer sb = new StringBuffer();
            sb.append("[");
            boolean flag = false;
            for (Message message : messages){
                sb.append(message.toJSON());
                sb.append(",");
                flag = true;
                message.setStatus(0);
                messageService.saveChange(message);
            }
            if (flag){
                sb.deleteCharAt(sb.length() - 1);
            }
            sb.append("]");
            response.getWriter().write(sb.toString());
        }
        SessionThreadLocal.closeSession();
        return NONE;
    }

    public String getTargetId() {
        return targetId;
    }

    public void setTargetId(String targetId) {
        this.targetId = targetId;
    }

    public String getContent() {
        return content;
    }

    public void setContent(String content) {
        this.content = content;
    }
}
