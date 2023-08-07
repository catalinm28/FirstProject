﻿using Dapper;
using FirstProjectRepository.DBModels;
using FirstProjectRepository.UsefullModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstProjectRepository.Repository
{
    public class PostRepository
    {
        private readonly IDbConnection _connection;
        private readonly CommentRepository _commentRepository;


        public PostRepository(IDbConnection connection, CommentRepository commentRepository)
        {
            _connection = connection;
            _commentRepository = commentRepository;

        }
        public Post GetPostById(int id)
        {
            string procedureName = "GetPostById";
            var parameters = new { postId = id };

            return _connection.QueryFirstOrDefault<Post>(
                procedureName,
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }
        public List<Post> GetPostByUserId(Guid userToken)
        {
            string procedureName = "GetPostByUserId";
            var parameters = new { userToken = userToken };
            return _connection.Query<Post>(procedureName, parameters, commandType: CommandType.StoredProcedure).ToList();
        }
        public List<Post> GetAllPosts()
        {
            string procedureName = "GetAllPosts";
            return _connection.Query<Post>(procedureName, commandType: CommandType.StoredProcedure).ToList();
        }
        public void InsertPost(Post post)
        {
            string procedureName = "InsertPost";
            var parameters = new
            {
                userToken = post.userToken,
                title = post.title,
                text = post.text,
                url = post.url,
                createdAt = post.createdAt
            };

            _connection.Execute(procedureName, parameters, commandType: CommandType.StoredProcedure);

        }
        public void UpdatePost(Post post)
        {
            string procedureName = "UpdatePost";
            var parameters = new
            {
                postId = post.postId,
                userToken = post.userToken,
                title = post.title,
                text = post.text,
                url = post.url,
                createdAt = post.createdAt
            };

            _connection.Execute(procedureName, parameters, commandType: CommandType.StoredProcedure);
        }
        public void DeletePost(int id)
        {
            string procedureName = "DeletePost";
            var parameter = new { postId = id };
            _connection.Execute(procedureName, parameter, commandType: CommandType.StoredProcedure);
        }
        public List<PostWithUsername> GetPostsWithUsername()
        {
            string procedureName = "GetPostsWithUsername";
            return _connection.Query<PostWithUsername>(procedureName, commandType: CommandType.StoredProcedure).ToList();
        }
        public PostWithUsername GetPostWithUsername(int postId)
        {
            string procedureName = "GetPostWithUsername";
            return _connection.QueryFirstOrDefault<PostWithUsername>(procedureName, new { postId }, commandType: CommandType.StoredProcedure);
        }
        public List<PostWithUsername> GetAllPostsSortedByDate()
        {
            string procedureName = "GetAllPostsSortedByDateWithCommentsCount";
            return _connection.Query<PostWithUsername>(procedureName, commandType: CommandType.StoredProcedure).ToList();
        }

        public PostWithUsername GetPostAndComments(int id)
        {
            var post = GetPostWithUsername(id);
            var comments = _commentRepository.GetPostWithCommentsAndReplies(id);

            
            if (post == null)
            {
                
                return null;
            }

            
            if (comments == null)
            {
                
                return post; 
            }

            OrganizeCommentsAndReplies(comments);
            post.comments = comments;
            return post;
        }

        private void OrganizeCommentsAndReplies(List<CommentWithReplies> comments)
        {

            Dictionary<int, CommentWithReplies> commentsById = comments.ToDictionary(c => c.id);


            foreach (var comment in comments)
            {
                if (comment.commentId.HasValue && commentsById.TryGetValue(comment.commentId.Value, out var parentComment))
                {
                   
                    parentComment.replies.Add(comment);
                }
            }
        }
    }
}
