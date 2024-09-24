//using DA.GtSWB.Persistence.Extensions;

//namespace DA.GtSWB.Tests.UnitTests.Persistence.Extensions;
//public class IQueryableExtensionsTests
//{
//    private record TestEntity(int Id, int Value, decimal Price);

//    [Fact]
//    public async Task MaxOrDefaultAsync_Should_ReturnMax_WhenQueryableIsNotEmpty()
//    {
//        var data = new List<TestEntity>
//        {
//            new(1,10,5.20m),
//            new(1,20,5.20m),
//            new(1,30,4.10m)
//        }.AsQueryable();

//        var result = await data.MaxOrDefaultAsync(e => e.Value, fallback: -1);

//        result.ShouldBe(30);
//    }

//    [Fact]
//    public async Task MaxOrDefaultAsync_Should_ReturnFallback_WhenQueryableIsEmpty()
//    {
//        var data = new List<TestEntity>().AsQueryable();

//        var result = await data.MaxOrDefaultAsync(e => e.Price, fallback: 100.20m);

//        result.ShouldBe(100.20m);
//    }

//    [Fact]
//    public async Task MaxOrDefaultAsync_Should_ThrowTaskCanceledException_WhenCancelled()
//    {
//        // Arrange
//        var data = new List<TestEntity>
//        {
//            new(1,10,5.20m)
//        }.AsQueryable();

//        var cancellationTokenSource = new CancellationTokenSource();
//        cancellationTokenSource.Cancel();

//        // Act & Assert
//        await Should.ThrowAsync<TaskCanceledException>(
//            async () => await data.MaxOrDefaultAsync(e => e.Value, fallback: 0, cancellationTokenSource.Token)
//        );
//    }

//    [Fact]
//    public async Task MaxOrNullableAsync_Should_ReturnMax_WhenQueryableIsNotEmpty()
//    {
//        var data = new List<TestEntity>
//        {
//            new(1,10,5.20m),
//            new(1,20,5.20m),
//            new(1,30,4.10m)
//        }.AsQueryable();

//        var result = await data.MaxOrNullableAsync(e => e.Price);

//        result.ShouldBe(5.20m);
//    }

//    [Fact]
//    public async Task MaxOrNullableAsync_Should_ReturnNull_WhenQueryableIsEmpty()
//    {
//        var data = new List<TestEntity>().AsQueryable();

//        var result = await data.MaxOrNullableAsync(e => e.Value);

//        result.ShouldBe(default);
//    }

//    [Fact]
//    public async Task MaxOrNullableAsync_Should_ThrowTaskCanceledException_WhenCancelled()
//    {
//        // Arrange
//        var data = new List<TestEntity>
//        {
//            new(1,10,5.20m)
//        }.AsQueryable();

//        var cancellationTokenSource = new CancellationTokenSource();
//        cancellationTokenSource.Cancel();

//        // Act & Assert
//        await Should.ThrowAsync<TaskCanceledException>(
//            async () => await data.MaxOrNullableAsync(e => e.Value, cancellationTokenSource.Token)
//        );
//    }
//}
