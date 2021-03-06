﻿using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SharpBucket.Utility;
using SharpBucket.V2.Pocos;

namespace SharpBucket.V2.EndPoints
{
    public class TeamResource : EndPoint
    {
        private readonly string _teamName;

        private readonly Lazy<RepositoriesAccountResource> _repositoriesResource;

        [Obsolete("Prefer new TeamsEndPoint(sharpBucketV2).TeamResource(teamName) or sharpBucketV2.TeamsEndPoint().TeamResource(teamName)")]
        public TeamResource(ISharpBucketRequesterV2 sharpBucketV2, string teamName)
            : this(new TeamsEndPoint(sharpBucketV2), teamName)
        {
        }

        internal TeamResource(TeamsEndPoint teamsEndPoint, string teamName)
            : base(teamsEndPoint, teamName.CheckIsNotNullNorEmpty(nameof(teamName)).GuidOrValue())
        {
            _teamName = teamName;

            _repositoriesResource = new Lazy<RepositoriesAccountResource>(
                () => new RepositoriesEndPoint(_sharpBucketV2).RepositoriesResource(teamName));
        }

        /// <summary>
        /// Gets the <see cref="RepositoriesAccountResource"/> corresponding to the team of this resource.
        /// </summary>
        /// <remarks>
        /// The /teams/{username}/repositories request redirect to the /repositories/{username} request
        /// It's why providing here a shortcut to the /repositories/{username} resource is valid and equivalent.
        /// </remarks>
        public RepositoriesAccountResource RepositoriesResource => _repositoriesResource.Value;

        /// <summary>
        /// Gets the public information associated with a team. 
        /// If the team's profile is private, the caller must be authenticated and authorized to view this information. 
        /// </summary>
        public Team GetProfile()
        {
            return _sharpBucketV2.Get<Team>(_baseUrl);
        }

        /// <summary>
        /// Gets the public information associated with a team. 
        /// If the team's profile is private, the caller must be authenticated and authorized to view this information. 
        /// </summary>
        /// <param name="token">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        public async Task<Team> GetProfileAsync(CancellationToken token = default)
        {
            return await _sharpBucketV2.GetAsync<Team>(_baseUrl, token);
        }

        /// <summary>
        /// Gets the team's members.
        /// </summary>
        /// <param name="max">The maximum number of items to return. 0 returns all items.</param>
        /// <returns></returns>
        public List<UserShort> ListMembers(int max = 0)
        {
            var overrideUrl = _baseUrl + "members/";
            return _sharpBucketV2.GetPaginatedValues<UserShort>(overrideUrl, max);
        }

        /// <summary>
        /// Enumerate the team's members,
        /// doing requests page by page while enumerating.
        /// </summary>
        /// <param name="pageLen">
        /// The length of a page. If not defined the default page length will be used
        /// </param>
        public IEnumerable<UserShort> EnumerateMembers(int? pageLen = null)
        {
            var overrideUrl = _baseUrl + "members/";
            return _sharpBucketV2.EnumeratePaginatedValues<UserShort>(overrideUrl, pageLen: pageLen);
        }

#if CS_8
        /// <summary>
        /// Enumerate the team's members,
        /// doing requests page by page while enumerating.
        /// </summary>
        /// <param name="token">
        /// A cancellation token that can be used by other objects or threads to receive notice of cancellation.
        /// </param>
        public IAsyncEnumerable<UserShort> EnumerateMembersAsync(CancellationToken token = default)
            => EnumerateMembersAsync(null, token);

        /// <summary>
        /// Enumerate the team's members,
        /// doing requests page by page while enumerating.
        /// </summary>
        /// <param name="pageLen">
        /// The length of a page. If not defined the default page length will be used
        /// </param>
        /// <param name="token">
        /// A cancellation token that can be used by other objects or threads to receive notice of cancellation.
        /// </param>
        public IAsyncEnumerable<UserShort> EnumerateMembersAsync(int? pageLen, CancellationToken token = default)
        {
            var overrideUrl = _baseUrl + "members/";
            return _sharpBucketV2.EnumeratePaginatedValuesAsync<UserShort>(overrideUrl, null, pageLen, token);
        }
#endif

        /// <summary>
        /// Gets the list of accounts following the team.
        /// </summary>
        /// <param name="max">The maximum number of items to return. 0 returns all items.</param>
        /// <returns></returns>
        public List<UserShort> ListFollowers(int max = 0)
        {
            var overrideUrl = _baseUrl + "followers/";
            return _sharpBucketV2.GetPaginatedValues<UserShort>(overrideUrl, max);
        }

        /// <summary>
        /// Enumerate the accounts following the team,
        /// doing requests page by page while enumerating.
        /// </summary>
        /// <param name="pageLen">
        /// The length of a page. If not defined the default page length will be used
        /// </param>
        public IEnumerable<UserShort> EnumerateFollowers(int? pageLen = null)
        {
            var overrideUrl = _baseUrl + "followers/";
            return _sharpBucketV2.EnumeratePaginatedValues<UserShort>(overrideUrl, pageLen: pageLen);
        }

#if CS_8
        /// <summary>
        /// Enumerate the accounts following the team,
        /// doing requests page by page while enumerating.
        /// </summary>
        /// <param name="token">
        /// A cancellation token that can be used by other objects or threads to receive notice of cancellation.
        /// </param>
        public IAsyncEnumerable<UserShort> EnumerateFollowersAsync(CancellationToken token = default)
            => EnumerateFollowersAsync(null, token);

        /// <summary>
        /// Enumerate the accounts following the team,
        /// doing requests page by page while enumerating.
        /// </summary>
        /// <param name="pageLen">
        /// The length of a page. If not defined the default page length will be used
        /// </param>
        /// <param name="token">
        /// A cancellation token that can be used by other objects or threads to receive notice of cancellation.
        /// </param>
        public IAsyncEnumerable<UserShort> EnumerateFollowersAsync(int? pageLen, CancellationToken token = default)
        {
            var overrideUrl = _baseUrl + "followers/";
            return _sharpBucketV2.EnumeratePaginatedValuesAsync<UserShort>(overrideUrl, null, pageLen, token);
        }
#endif

        /// <summary>
        /// Gets a list of accounts the team is following.
        /// </summary>
        /// <param name="max">The maximum number of items to return. 0 returns all items.</param>
        /// <returns></returns>
        public List<UserShort> ListFollowing(int max = 0)
        {
            var overrideUrl = _baseUrl + "following/";
            return _sharpBucketV2.GetPaginatedValues<UserShort>(overrideUrl, max);
        }

        /// <summary>
        /// Enumerate the accounts the team is following,
        /// doing requests page by page while enumerating.
        /// </summary>
        /// <param name="pageLen">
        /// The length of a page. If not defined the default page length will be used
        /// </param>
        public IEnumerable<UserShort> EnumerateFollowing(int? pageLen = null)
        {
            var overrideUrl = _baseUrl + "following/";
            return _sharpBucketV2.EnumeratePaginatedValues<UserShort>(overrideUrl, pageLen: pageLen);
        }

#if CS_8
        /// <summary>
        /// Enumerate the accounts the team is following,
        /// doing requests page by page while enumerating.
        /// </summary>
        /// <param name="token">
        /// A cancellation token that can be used by other objects or threads to receive notice of cancellation.
        /// </param>
        public IAsyncEnumerable<UserShort> EnumerateFollowingAsync(CancellationToken token = default)
            => EnumerateFollowingAsync(null, token);

        /// <summary>
        /// Enumerate the accounts the team is following,
        /// doing requests page by page while enumerating.
        /// </summary>
        /// <param name="pageLen">
        /// The length of a page. If not defined the default page length will be used
        /// </param>
        /// <param name="token">
        /// A cancellation token that can be used by other objects or threads to receive notice of cancellation.
        /// </param>
        public IAsyncEnumerable<UserShort> EnumerateFollowingAsync(int? pageLen, CancellationToken token = default)
        {
            var overrideUrl = _baseUrl + "following/";
            return _sharpBucketV2.EnumeratePaginatedValuesAsync<UserShort>(overrideUrl, null, pageLen, token);
        }
#endif

        /// <summary>
        /// List of repositories associated to the team.
        /// Private repositories only appear on this list if the caller is authenticated and is authorized to view the repository.
        /// </summary>
        /// <param name="parameters">Parameters for the query.</param>
        [Obsolete("Prefer go through the RepositoriesResource property.")]
        public List<Repository> ListRepositories(ListParameters parameters)
            => new RepositoriesEndPoint(_sharpBucketV2).ListRepositories(_teamName, parameters ?? new ListParameters());


        /// <summary>
        /// Gets a <see cref="RepositoryResource"/> for a specified repository name, owned by the team represented by this resource.
        /// </summary>
        /// <param name="repoSlugOrName">The repository slug, name, or UUID.</param>
        [Obsolete("Prefer go through the RepositoriesResource property.")]
        public RepositoryResource RepositoryResource(string repoSlugOrName)
        {
            return RepositoriesResource.RepositoryResource(repoSlugOrName);
        }

        /// <summary>
        /// Gets a list of projects that belong to the team.
        /// https://developer.atlassian.com/bitbucket/api/2/reference/resource/teams/%7Busername%7D/projects/#get
        /// </summary>
        /// <param name="max">The maximum number of items to return. 0 returns all items.</param>
        public List<Project> ListProjects(int max = 0)
            => ListProjects(new ListParameters { Max = max });

        public List<Project> ListProjects(ListParameters parameters)
        {
            _ = parameters ?? throw new ArgumentNullException(nameof(parameters));
            var overrideUrl = _baseUrl + "projects/";
            return _sharpBucketV2.GetPaginatedValues<Project>(overrideUrl, parameters.Max, parameters.ToDictionary());
        }

        /// <summary>
        /// Enumerate projects that belong to the team,
        /// doing requests page by page while enumerating.
        /// https://developer.atlassian.com/bitbucket/api/2/reference/resource/teams/%7Busername%7D/projects/#get
        /// </summary>
        public IEnumerable<Project> EnumerateProjects()
            => EnumerateProjects(new EnumerateParameters());

        /// <summary>
        /// Enumerate projects that belong to the team,
        /// doing requests page by page while enumerating.
        /// https://developer.atlassian.com/bitbucket/api/2/reference/resource/teams/%7Busername%7D/projects/#get
        /// </summary>
        /// <param name="parameters">Parameters for the query.</param>
        public IEnumerable<Project> EnumerateProjects(EnumerateParameters parameters)
        {
            _ = parameters ?? throw new ArgumentNullException(nameof(parameters));
            var overrideUrl = _baseUrl + "projects/";
            return _sharpBucketV2.EnumeratePaginatedValues<Project>(overrideUrl, parameters.ToDictionary(), parameters.PageLen);
        }

#if CS_8
        /// <summary>
        /// Enumerate projects that belong to the team,
        /// doing requests page by page while enumerating.
        /// https://developer.atlassian.com/bitbucket/api/2/reference/resource/teams/%7Busername%7D/projects/#get
        /// </summary>
        public IAsyncEnumerable<Project> EnumerateProjectsAsync(CancellationToken token = default)
            => EnumerateProjectsAsync(new EnumerateParameters(), token);

        /// <summary>
        /// Enumerate projects that belong to the team,
        /// doing requests page by page while enumerating.
        /// https://developer.atlassian.com/bitbucket/api/2/reference/resource/teams/%7Busername%7D/projects/#get
        /// </summary>
        /// <param name="parameters">Parameters for the query.</param>
        public IAsyncEnumerable<Project> EnumerateProjectsAsync(EnumerateParameters parameters, CancellationToken token = default)
        {
            _ = parameters ?? throw new ArgumentNullException(nameof(parameters));
            var overrideUrl = _baseUrl + "projects/";
            return _sharpBucketV2.EnumeratePaginatedValuesAsync<Project>(overrideUrl, parameters.ToDictionary(), parameters.PageLen, token);
        }
#endif

        /// <summary>
        /// Create a new project.
        /// https://developer.atlassian.com/bitbucket/api/2/reference/resource/teams/%7Busername%7D/projects/
        /// </summary>
        /// <param name="project"></param>
        /// <returns>A new project instance that fully represent the newly created project.</returns>
        public Project PostProject(Project project)
        {
            var overrideUrl = _baseUrl + "projects/";
            return _sharpBucketV2.Post(project, overrideUrl);
        }

        /// <summary>
        /// Create a new project.
        /// https://developer.atlassian.com/bitbucket/api/2/reference/resource/teams/%7Busername%7D/projects/
        /// </summary>
        /// <param name="project"></param>
        /// <param name="token">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>A new project instance that fully represent the newly created project.</returns>
        public async Task<Project> PostProjectAsync(Project project, CancellationToken token = default)
        {
            var overrideUrl = _baseUrl + "projects/";
            return await _sharpBucketV2.PostAsync(project, overrideUrl, token);
        }

        /// <summary>
        /// Gets a <see cref="ProjectResource"/> for a specified project key.
        /// </summary>
        /// <param name="projectKey">This can either be the actual key assigned to the project or the UUID.</param>
        public ProjectResource ProjectResource(string projectKey)
        {
            return new ProjectResource(this, projectKey);
        }

        /// <summary>
        /// Searches for code in team account repositories, and lazily enumerate the search results.
        /// https://developer.atlassian.com/bitbucket/api/2/reference/resource/teams/%7Busername%7D/search/code
        /// </summary>
        /// <param name="searchQuery">The string that is passed as search query.</param>
        /// <param name="pageLen">The length of a page. If not defined the default page length will be used.</param>
        /// <returns>A lazy enumerable that will request results pages by pages while enumerating the results.</returns>
        public IEnumerable<SearchCodeSearchResult> EnumerateSearchCodeSearchResults(
            string searchQuery,
            int? pageLen = null)
        {
            var overrideUrl = $"{_baseUrl}search/code";
            var requestParameters = new Dictionary<string, object>
            {
                { "search_query", searchQuery }
            };
            return _sharpBucketV2.EnumeratePaginatedValues<SearchCodeSearchResult>(overrideUrl, requestParameters, pageLen);
        }
    }
}
