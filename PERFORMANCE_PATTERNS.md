# Performance Optimization Patterns - AuxoniaManage

This document catalogs performance optimization patterns used throughout the codebase for future reference and consistency.

## 🚀 Core Optimization Patterns

### Pattern 1: Batch + Dictionary Lookup
**Location**: `GetTasksQueryHandler.cs:80-110`  
**Problem**: Multiple database calls for profile data  
**Solution**: Single batch fetch + dictionary lookup

```csharp
// ❌ Bad: N database calls
foreach (var task in tasks) {
    var profile = await _repo.GetProfileAsync(task.AssignedById);  // N queries!
}

// ✅ Good: 1 database call + O(1) lookups
var allUserIds = tasks.SelectMany(t => t.AssigneeIds).ToList();
var profiles = await _repo.GetProfilesByUserIds(allUserIds);
var profilesDict = new Dictionary<string, ProfileDto>();
foreach (var profile in profiles) {
    profilesDict[profile.UserId] = MapToDto(profile);
}

// Fast lookups
var assignedBy = profilesDict[task.AssignedById];  // O(1)
```

**Performance Impact**: 50-100x faster than individual queries  
**Use When**: Multiple related lookups needed

### Pattern 2: HashSet for Set Operations  
**Location**: `WorkspacePermissionService.cs:75-77`  
**Problem**: Finding missing elements efficiently  
**Solution**: HashSet operations for set difference

```csharp
// ❌ Bad: Nested loops O(n²)
var missing = new List<string>();
foreach (var expected in expectedIds) {
    if (!foundIds.Contains(expected)) {  // O(n) each time
        missing.Add(expected);
    }
}

// ✅ Good: Set operations O(n)
var expectedUserIds = userIds.ToHashSet();           // O(n)
var foundUserIds = memberships.Select(m => m.UserId).ToHashSet();  // O(n)
var missingUserIds = expectedUserIds.Except(foundUserIds);        // O(n)
```

**Performance Impact**: O(n) instead of O(n²)  
**Use When**: Set operations (difference, intersection, union)

### Pattern 3: Static HashSet for Constants
**Location**: `AllowFile.cs:5`  
**Problem**: Repeated string comparisons for validation  
**Solution**: Pre-computed HashSet with case-insensitive comparison

```csharp
// ❌ Bad: Array search O(n)
private static readonly string[] _allowedExtensions = { ".jpg", ".jpeg", ".png" };
public bool IsValid(string ext) {
    return _allowedExtensions.Contains(ext);  // O(n) linear search
}

// ✅ Good: HashSet lookup O(1)
private static readonly IReadOnlySet<string> _allowedExtensions = 
    new HashSet<string>(StringComparer.OrdinalIgnoreCase) {
        ".jpg", ".jpeg", ".png"
    };

public bool IsValid(string ext) {
    return _allowedExtensions.Contains(ext);  // O(1) hash lookup
}
```

**Performance Impact**: O(1) vs O(n) for validation  
**Use When**: Static lookups, case-insensitive comparisons

## 📊 Performance Comparison Table

| Pattern | Data Structure | Lookup Time | Best Use Case | Memory Overhead |
|---------|---------------|-------------|---------------|-----------------|
| Array/List | `List<T>` | O(n) | Small datasets (<10) | Low |
| Dictionary | `Dictionary<K,V>` | O(1) | Key-value lookups | Medium |
| HashSet | `HashSet<T>` | O(1) | Set operations | Medium |
| Static HashSet | `IReadOnlySet<T>` | O(1) | Constant validations | Low |

## 🎯 When to Use Each Pattern

### Use Dictionary When:
- Multiple lookups by key needed
- Mapping relationships exist
- Performance-critical lookup paths
- Data size > 10 items

### Use HashSet When:  
- Set operations needed (union, intersection, difference)
- Uniqueness validation required
- Fast membership testing
- No key-value relationship

### Use Static Collections When:
- Configuration/validation constants
- Read-only reference data
- Application-wide lookups
- Thread-safe access needed

## 🔍 Pattern Detection Checklist

### 🚨 Performance Anti-Patterns to Avoid:
- `list.Contains()` in loops
- `list.FirstOrDefault(x => x.Id == id)` repeatedly
- Multiple database calls for related data
- Nested loops for set operations

### ✅ Optimization Opportunities:
- Multiple lookups by same criteria
- Repeated membership tests
- Set difference/intersection operations
- Validation against static lists

## 📈 Measuring Impact

### Before Optimization:
```csharp
// Measure this pattern:
foreach (var item in items) {
    var result = expensiveList.FirstOrDefault(x => x.Id == item.Id);  // O(n) each
}
// Total: O(n × m) where n=items, m=expensiveList size
```

### After Optimization:
```csharp
// Measure this pattern:
var lookup = expensiveList.ToDictionary(x => x.Id);  // O(m) once
foreach (var item in items) {
    var result = lookup[item.Id];  // O(1) each
}
// Total: O(n + m) - much better!
```

## 🎨 Code Templates

### Batch + Dictionary Template:
```csharp
// 1. Collect all keys
var allKeys = entities.SelectMany(GetKeys).Distinct().ToList();

// 2. Batch fetch
var lookupData = await repository.GetManyAsync(allKeys);

// 3. Build dictionary  
var lookupDict = lookupData.ToDictionary(x => x.Key);

// 4. Use fast lookups
entities.Select(entity => new DTO {
    RelatedData = GetKeys(entity).Select(key => lookupDict[key]).ToList()
});
```

### Set Operations Template:
```csharp
var expectedSet = expected.ToHashSet();
var actualSet = actual.Select(GetKey).ToHashSet();

var missing = expectedSet.Except(actualSet);
var extra = actualSet.Except(expectedSet);
var intersection = expectedSet.Intersect(actualSet);
```

## 🏷️ Tags
`#performance` `#optimization` `#dictionary` `#hashset` `#batch-processing` `#o1-lookup`

---
*Last updated: Generated by Claude Code*  
*Remember: Premature optimization is the root of all evil, but knowing these patterns helps you optimize when it actually matters!*