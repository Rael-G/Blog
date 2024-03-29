﻿using AutoMapper;
using Blog.Application;
using Blog.Domain;
using Moq;
using FluentAssertions;

namespace Blog.UnitTest.Application.Services
{
    public class PostServiceTests
    {
        private readonly Mock<IPostRepository> _mockRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly PostService _postService;

        private readonly Post _post;

        public PostServiceTests()
        {
            _mockRepository = new Mock<IPostRepository>();
            _mockMapper = new Mock<IMapper>();
            _postService = new PostService(_mockRepository.Object, _mockMapper.Object);

            var createdTime = DateTime.Now.Subtract(TimeSpan.FromMinutes(5));
            var updateTime = DateTime.Now;

            _post = new(Guid.NewGuid(), createdTime, updateTime, "Title", "Content", []);
        }

        [Fact]
        public void Create_Should_Call_Validate()
        {
            var postDto = new PostDto()
            { Title = "Title", Content = "Content" };

            var invalidPost = _post;
            invalidPost.Title = "";

            _mockMapper.Setup(m => m.Map<Post>(It.IsAny<PostDto>()))
                .Returns(invalidPost);

            //If the exception was throw here, it is because Validate() was Called
            Assert.Throws<ArgumentException>(() => _postService.Create(postDto));
        }

        [Fact]
        public void Create_Should_Call_Repository_Create()
        {
            var postDto = new PostDto()
            { Title = "Title", Content = "Content" };

            _mockMapper.Setup(m => m.Map<Post>(It.IsAny<PostDto>()))
                .Returns(_post);

            _postService.Create(postDto);

            _mockRepository.Verify(m => m.Create(_post), Times.Once);
        }

        [Fact]
        public void Create_Should_DefineTime()
        {
            var creationTime = DateTime.UtcNow;
            var postDto = new PostDto()
            { Title = "Title", Content = "Content" };

            _mockMapper.Setup(m => m.Map<Post>(It.IsAny<PostDto>()))
                .Returns(_post);

            _postService.Create(postDto);

            postDto.CreatedTime.Should().BeAfter(creationTime);
            postDto.UpdateTime.Should().BeAfter(creationTime);
        }

        [Fact]
        public void Update_Should_Call_Validate()
        {
            var postDto = new PostDto()
            { Title = "Title", Content = "Content" };

            var invalidPost = _post;
            invalidPost.Title = "";

            _mockMapper.Setup(m => m.Map<Post>(It.IsAny<PostDto>()))
                .Returns(invalidPost);

            //If the exception was throw here, it is because Validate() was Called
            Assert.Throws<ArgumentException>(() => _postService.Update(postDto));
        }

        [Fact]
        public void Update_Should_Call_Repository_Update()
        {
            var postDto = new PostDto()
            { Id = _post.Id, Title = "Updated Title", Content = "Updated Content" };

            _mockMapper.Setup(m => m.Map<Post>(It.IsAny<PostDto>()))
                .Returns(_post);

            _postService.Update(postDto);

            _mockRepository.Verify(m => m.Update(_post), Times.Once);
        }

        [Fact]
        public void Update_Should_DefineTime()
        {
            var creationTime = DateTime.UtcNow;
            var postDto = new PostDto()
            { Title = "Title", Content = "Content" };

            _mockMapper.Setup(m => m.Map<Post>(It.IsAny<PostDto>()))
                .Returns(_post);

            _postService.Update(postDto);

            postDto.UpdateTime.Should().BeAfter(creationTime);
        }

        [Fact]
        public void Delete_Should_Call_Repository_Delete()
        {
            var postDto = new PostDto();

            _postService.Delete(postDto);

            _mockRepository.Verify(m => m.Delete(It.IsAny<Post>()), Times.Once);
        }

        [Fact]
        public async Task Get_Should_Call_Repository_Get_And_Map_To_Dto()
        {
            var postDto = new PostDto()
            { Id = _post.Id, Title = _post.Title, Content = _post.Content };

            _mockRepository.Setup(m => m.Get(It.IsAny<Guid>()))
                .ReturnsAsync(_post);
            _mockMapper.Setup(m => m.Map<PostDto>(_post))
                .Returns(postDto);

            var result = await _postService.Get(_post.Id);

            result.Should().BeEquivalentTo(postDto);
            _mockRepository.Verify(m => m.Get(_post.Id), Times.Once);
        }

        [Fact]
        public async Task GetAll_Should_Call_Repository_GetAll_And_Map_To_DtoCollection()
        {
            var posts = new List<Post>();
            var postDtos = new List<PostDto>();

            _mockRepository.Setup(m => m.GetAll()).ReturnsAsync(posts);
            _mockMapper.Setup(m => m.Map<IEnumerable<PostDto>>(posts))
                .Returns(postDtos);

            var result = await _postService.GetAll();

            result.Should().BeEquivalentTo(postDtos);
            _mockRepository.Verify(m => m.GetAll(), Times.Once);
        }
    }
}
