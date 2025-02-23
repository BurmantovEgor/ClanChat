using AutoMapper;
using ClanChat.Abstractions.Clan;
using ClanChat.Core.DTOs.Clan;
using ClanChat.Core.Models;
using ClanChat.Core.Services;
using ClanChat.Data.Entities;
using Moq;

public class ClanServiceTests
{
    private readonly ClanService _clanService;
    private readonly Mock<IClanRepository> _clanRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;

    public ClanServiceTests()
    {
        _clanRepositoryMock = new Mock<IClanRepository>();
        _mapperMock = new Mock<IMapper>();
        _clanService = new ClanService(_mapperMock.Object, _clanRepositoryMock.Object);
    }

    [Fact]
    public async Task CreateNewAsyncSuccess()
    {
        var createClanDto = new CreateClanDTO { Name = "TestClan", Description = "TestClan Desc" };
        var clanEntity = new ClanEntity { Id = Guid.NewGuid(), Name = createClanDto.Name };
        var clanDto = new ClanDTO { Id = clanEntity.Id, Name = clanEntity.Name, Description = clanEntity.Description };

        _clanRepositoryMock.Setup(repo => repo.FindByNameAsync(createClanDto.Name))
            .ReturnsAsync((ClanDTO)null);

        _mapperMock.Setup(mapper => mapper.Map<ClanEntity>(It.IsAny<ClanModel>()))
            .Returns(clanEntity);

        _clanRepositoryMock.Setup(repo => repo.CreateNewAsync(It.IsAny<ClanEntity>()))
            .ReturnsAsync(1);

        _clanRepositoryMock.Setup(repo => repo.FindByIdAsync(clanEntity.Id))
            .ReturnsAsync(clanDto);

        var result = await _clanService.CreateNewAsync(createClanDto);

        Assert.True(result.IsSuccess);
        Assert.Equal(clanDto.Id, result.Value.Id);
    }

    [Fact]
    public async Task CreateNewAsyncErrorWhenClanNameExists()
    {
        var createClanDto = new CreateClanDTO { Name = "TestClan", Description = "TestClan Desc" };
        var existingClanDto = new ClanDTO { Id = Guid.NewGuid(), Name = createClanDto.Name, Description = createClanDto.Description };

        _clanRepositoryMock.Setup(repo => repo.FindByNameAsync(createClanDto.Name))
            .ReturnsAsync(existingClanDto);

        var result = await _clanService.CreateNewAsync(createClanDto);

        Assert.True(result.IsFailure);
        Assert.Equal("Клан с таким именем уже существует", result.Error);
    }

    [Fact]
    public async Task CreateNewAsyncErrorWhenDBError()
    {
        var createClanDto = new CreateClanDTO { Name = "TestClan", Description = "TestClan Desc" };
        var clanEntity = new ClanEntity { Id = Guid.NewGuid(), Name = createClanDto.Name };

        _clanRepositoryMock.Setup(repo => repo.FindByNameAsync(createClanDto.Name))
            .ReturnsAsync((ClanDTO)null);

        _mapperMock.Setup(mapper => mapper.Map<ClanEntity>(It.IsAny<ClanModel>()))
            .Returns(clanEntity);

        _clanRepositoryMock.Setup(repo => repo.CreateNewAsync(It.IsAny<ClanEntity>()))
            .ReturnsAsync(0);

        var result = await _clanService.CreateNewAsync(createClanDto);

        Assert.True(result.IsFailure);
        Assert.Equal("Ошибка сохранения в БД", result.Error);
    }

    [Fact]
    public async Task FindByIdAsyncSuccess()
    {
        var clanId = Guid.NewGuid();
        var clanDto = new ClanDTO { Id = clanId, Name = "TestClan", Description = "TestClan Desc" };

        _clanRepositoryMock.Setup(repo => repo.FindByIdAsync(clanId))
            .ReturnsAsync(clanDto);

        var result = await _clanService.FindByIdAsync(clanId);

        Assert.True(result.IsSuccess);
        Assert.Equal(clanId, result.Value.Id);
    }


    [Fact]
    public async Task FindByIdAsyncErrorWhenClanNotExist()
    {
        var clanId = Guid.NewGuid();

        _clanRepositoryMock.Setup(repo => repo.FindByIdAsync(clanId))
            .ReturnsAsync((ClanDTO)null);

        var result = await _clanService.FindByIdAsync(clanId);

        Assert.True(result.IsFailure);
        Assert.Equal($"Клан с ID {clanId} не найден", result.Error);
    }

    [Fact]
    public async Task GetAllAsyncSuccess()
    {
        var clans = new List<ClanDTO>
        {
            new ClanDTO { Id = Guid.NewGuid(), Name = "TestClan1", Description = "TestClan1 Desc" },
            new ClanDTO { Id = Guid.NewGuid(), Name = "TestClan2", Description = "TestClan2 Desc" }
        };

        _clanRepositoryMock.Setup(repo => repo.GetAllAsync())
            .ReturnsAsync(clans);

        var result = await _clanService.GetAllAsync();

        Assert.True(result.IsSuccess);
        Assert.Equal(2, result.Value.Count);
    }


    [Fact]
    public async Task GetAllAsyncErrorWhenNoClansExist()
    {
        _clanRepositoryMock.Setup(repo => repo.GetAllAsync())
            .ReturnsAsync(new List<ClanDTO>());

        var result = await _clanService.GetAllAsync();

        Assert.True(result.IsFailure);
        Assert.Equal("Кланы не найдены", result.Error);
    }
}
